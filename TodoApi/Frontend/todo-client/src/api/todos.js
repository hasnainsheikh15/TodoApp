export const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL?.replace(/\/$/, '') ||
  'http://localhost:5264'

const TODOS_URL = `${API_BASE_URL}/api/Todo`

async function parseError(response) {
  try {
    const payload = await response.json()

    if (payload?.errors) {
      return Object.values(payload.errors).flat().join(' ')
    }

    return payload?.message || payload?.title || response.statusText
  } catch {
    return response.statusText
  }
}

async function request(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    headers: {
      Accept: 'application/json',
      ...(options.body ? { 'Content-Type': 'application/json' } : {}),
      ...options.headers,
    },
  })

  if (!response.ok) {
    throw new Error(await parseError(response))
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

export function getTodos(filters = {}, signal) {
  const params = new URLSearchParams()

  if (filters.search?.trim()) {
    params.set('search', filters.search.trim())
  }

  if (filters.category) {
    params.set('category', filters.category)
  }

  if (filters.priority) {
    params.set('priority', filters.priority)
  }

  const query = params.toString()

  return request(query ? `${TODOS_URL}?${query}` : TODOS_URL, { signal })
}

export function createTodo(todo) {
  return request(TODOS_URL, {
    method: 'POST',
    body: JSON.stringify(todo),
  })
}

export function updateTodo(id, todo) {
  return request(`${TODOS_URL}/${id}`, {
    method: 'PUT',
    body: JSON.stringify(todo),
  })
}

export function deleteTodo(id) {
  return request(`${TODOS_URL}/${id}`, {
    method: 'DELETE',
  })
}
