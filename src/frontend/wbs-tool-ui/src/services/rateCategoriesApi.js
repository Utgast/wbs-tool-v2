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

export async function getRateCategories() {
  const response = await fetch(`${API_BASE_URL}/ratecategories`, {
    method: 'GET',
    headers: {
      Accept: 'application/json',
    },
  })

  return handleResponse(response, 'Stundensatzkategorien konnten nicht geladen werden')
}