const API_BASE_URL = 'http://localhost:5046/api'

async function handleResponse(response, defaultMessage) {
  if (!response.ok) {
    let errorMessage = defaultMessage

    try {
      const errorData = await response.json()

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
    } catch {
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

export async function getResourceAssignments(projectId, wbsNodeId) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!wbsNodeId) {
    throw new Error('WBS-Knoten-ID fehlt')
  }

  const response = await fetch(
    `${API_BASE_URL}/projects/${projectId}/wbs/${wbsNodeId}/assignments`,
    {
      method: 'GET',
      headers: {
        Accept: 'application/json',
      },
    }
  )

  return handleResponse(response, 'Ressourcen-Zuordnungen konnten nicht geladen werden')
}

export async function createResourceAssignment(projectId, wbsNodeId, payload) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!wbsNodeId) {
    throw new Error('WBS-Knoten-ID fehlt')
  }

  const response = await fetch(
    `${API_BASE_URL}/projects/${projectId}/wbs/${wbsNodeId}/assignments`,
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
      body: JSON.stringify(payload),
    }
  )

  return handleResponse(response, 'Ressourcen-Zuordnung konnte nicht angelegt werden')
}

export async function updateResourceAssignment(projectId, wbsNodeId, assignmentId, payload) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!wbsNodeId) {
    throw new Error('WBS-Knoten-ID fehlt')
  }

  if (!assignmentId) {
    throw new Error('Assignment-ID fehlt')
  }

  const response = await fetch(
    `${API_BASE_URL}/projects/${projectId}/wbs/${wbsNodeId}/assignments/${assignmentId}`,
    {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
      },
      body: JSON.stringify(payload),
    }
  )

  return handleResponse(response, 'Ressourcen-Zuordnung konnte nicht gespeichert werden')
}

export async function deactivateResourceAssignment(projectId, wbsNodeId, assignmentId) {
  if (!projectId) {
    throw new Error('Projekt-ID fehlt')
  }

  if (!wbsNodeId) {
    throw new Error('WBS-Knoten-ID fehlt')
  }

  if (!assignmentId) {
    throw new Error('Assignment-ID fehlt')
  }

  const response = await fetch(
    `${API_BASE_URL}/projects/${projectId}/wbs/${wbsNodeId}/assignments/${assignmentId}`,
    {
      method: 'DELETE',
      headers: {
        Accept: 'application/json',
      },
    }
  )

  return handleResponse(response, 'Ressourcen-Zuordnung konnte nicht deaktiviert werden')
}