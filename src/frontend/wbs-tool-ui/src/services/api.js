const API_BASE_URL = 'http://localhost:5046/api'

async function handleResponse(response, defaultMessage) {
  if (!response.ok) {
    let errorMessage = defaultMessage

    try {
      const errorData = await response.json()

      console.error('API error response:', {
        url: response.url,
        status: response.status,
        data: errorData,
      })

      if (typeof errorData === 'string') {
        errorMessage = errorData
      } else if (errorData?.message) {
        errorMessage = errorData.message
      } else if (errorData?.title) {
        errorMessage = errorData.title
      } else if (errorData?.errors) {
        const firstErrorKey = Object.keys(errorData.errors)[0]
        const firstErrorValue = errorData.errors[firstErrorKey]

        if (Array.isArray(firstErrorValue) && firstErrorValue.length > 0) {
          errorMessage = firstErrorValue[0]
        }
      } else {
        errorMessage = `HTTP ${response.status}: ${defaultMessage}`
      }
    } catch (parseError) {
      console.error('API error response could not be parsed as JSON:', {
        url: response.url,
        status: response.status,
        parseError,
      })

      errorMessage = `HTTP ${response.status}: ${defaultMessage}`
    }

    throw new Error(errorMessage)
  }

  const contentType = response.headers.get('content-type') || ''

  if (contentType.includes('application/json')) {
    return response.json()
  }

  return null
}

export async function getProjects() {
  const response = await fetch(`${API_BASE_URL}/projects`, {
    method: 'GET',
    headers: {
      Accept: 'application/json',
    },
  })

  return handleResponse(response, 'Fehler beim Laden der Projekte')
}

export async function getProjectDashboard(projectId) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/dashboard`, {
    method: 'GET',
    headers: {
      Accept: 'application/json',
    },
  })

  return handleResponse(response, 'Fehler beim Laden der Projektübersicht')
}

export async function getWbsTree(projectId) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/wbs/tree`, {
    method: 'GET',
    headers: {
      Accept: 'application/json',
    },
  })

  return handleResponse(response, 'Fehler beim Laden der WBS')
}

export async function createWbsNode(payload) {
  if (!payload?.projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  console.log('createWbsNode payload:', payload)

  const { projectId, ...requestBody } = payload

  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/wbs`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(requestBody),
  })

  return handleResponse(response, 'Das Element konnte nicht angelegt werden')
}

export async function updateWbsNode(projectId, nodeId, payload) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!nodeId) {
    throw new Error('Node-ID fehlt')
  }

  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/wbs/${nodeId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(payload),
  })

  return handleResponse(response, 'Der WBS-Knoten konnte nicht gespeichert werden')
}

export async function deactivateWbsNode(projectId, nodeId) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!nodeId) {
    throw new Error('Node-ID fehlt')
  }

  const response = await fetch(`${API_BASE_URL}/projects/${projectId}/wbs/${nodeId}`, {
    method: 'DELETE',
    headers: {
      Accept: 'application/json',
    },
  })

  return handleResponse(response, 'Der WBS-Knoten konnte nicht deaktiviert werden')
}