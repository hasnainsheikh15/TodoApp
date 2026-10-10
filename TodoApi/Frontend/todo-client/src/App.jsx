import { useEffect, useMemo, useState } from 'react'
import './App.css'
import {
  API_BASE_URL,
  createTodo,
  deleteTodo,
  getTodos,
  updateTodo,
} from './api/todos'

const priorities = ['Low', 'Medium', 'High']
const categories = ['Work', 'Personal']

const emptyForm = {
  title: '',
  description: '',
  priority: 'Medium',
  category: 'Work',
}

function App() {
  const [todos, setTodos] = useState([])
  const [filters, setFilters] = useState({
    search: '',
    category: '',
    priority: '',
  })
  const [form, setForm] = useState(emptyForm)
  const [editingId, setEditingId] = useState(null)
  const [isLoading, setIsLoading] = useState(true)
  const [isSaving, setIsSaving] = useState(false)
  const [mutatingId, setMutatingId] = useState(null)
  const [error, setError] = useState('')

  const editingTodo = useMemo(
    () => todos.find((todo) => todo.id === editingId),
    [editingId, todos],
  )

  const completedCount = todos.filter((todo) => todo.isCompleted).length

  useEffect(() => {
    const controller = new AbortController()
    const timer = window.setTimeout(async () => {
      setIsLoading(true)
      setError('')

      try {
        const data = await getTodos(filters, controller.signal)
        setTodos(data)
      } catch (error) {
        if (error.name !== 'AbortError') {
          setError(error.message || 'Unable to load todos.')
        }
      } finally {
        setIsLoading(false)
      }
    }, 250)

    return () => {
      window.clearTimeout(timer)
      controller.abort()
    }
  }, [filters])

  async function refreshTodos() {
    setIsLoading(true)
    setError('')

    try {
      const data = await getTodos(filters)
      setTodos(data)
    } catch (error) {
      setError(error.message || 'Unable to load todos.')
    } finally {
      setIsLoading(false)
    }
  }

  function updateFilter(event) {
    const { name, value } = event.target
    setFilters((current) => ({ ...current, [name]: value }))
  }

  function updateForm(event) {
    const { name, value } = event.target
    setForm((current) => ({ ...current, [name]: value }))
  }

  function resetForm() {
    setForm(emptyForm)
    setEditingId(null)
  }

  function startEditing(todo) {
    setEditingId(todo.id)
    setForm({
      title: todo.title,
      description: todo.description || '',
      priority: todo.priority,
      category: categories.includes(todo.category) ? todo.category : 'Work',
    })
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  async function handleSubmit(event) {
    event.preventDefault()

    if (!form.title.trim()) {
      setError('Title is required.')
      return
    }

    const payload = {
      title: form.title.trim(),
      description: form.description.trim() || null,
      priority: form.priority,
      category: form.category,
    }

    setIsSaving(true)
    setError('')

    try {
      if (editingId) {
        await updateTodo(editingId, {
          ...payload,
          isCompleted: editingTodo?.isCompleted ?? false,
        })
      } else {
        await createTodo(payload)
      }

      resetForm()
      await refreshTodos()
    } catch (error) {
      setError(error.message || 'Unable to save todo.')
    } finally {
      setIsSaving(false)
    }
  }

  async function toggleCompleted(todo) {
    setMutatingId(todo.id)
    setError('')

    try {
      await updateTodo(todo.id, {
        title: todo.title,
        description: todo.description,
        priority: todo.priority,
        category: todo.category,
        isCompleted: !todo.isCompleted,
      })
      await refreshTodos()
    } catch (error) {
      setError(error.message || 'Unable to update todo.')
    } finally {
      setMutatingId(null)
    }
  }

  async function removeTodo(todo) {
    if (!window.confirm(`Delete "${todo.title}"?`)) {
      return
    }

    setMutatingId(todo.id)
    setError('')

    try {
      await deleteTodo(todo.id)

      if (editingId === todo.id) {
        resetForm()
      }

      await refreshTodos()
    } catch (error) {
      setError(error.message || 'Unable to delete todo.')
    } finally {
      setMutatingId(null)
    }
  }

  return (
    <main className="app-shell">
      <header className="app-header">
        <div>
          <p className="eyebrow">Todo API frontend</p>
          <h1>Todos</h1>
        </div>
        <div className="api-pill">API {API_BASE_URL}</div>
      </header>

      <section className="toolbar" aria-label="Todo filters">
        <label>
          Search
          <input
            name="search"
            value={filters.search}
            onChange={updateFilter}
            placeholder="Title or description"
          />
        </label>

        <label>
          Category
          <select name="category" value={filters.category} onChange={updateFilter}>
            <option value="">All</option>
            {categories.map((category) => (
              <option key={category} value={category}>
                {category}
              </option>
            ))}
          </select>
        </label>

        <label>
          Priority
          <select name="priority" value={filters.priority} onChange={updateFilter}>
            <option value="">All</option>
            {priorities.map((priority) => (
              <option key={priority} value={priority}>
                {priority}
              </option>
            ))}
          </select>
        </label>
      </section>

      <section className="form-section" aria-labelledby="todo-form-heading">
        <div className="section-heading">
          <h2 id="todo-form-heading">
            {editingId ? 'Edit todo' : 'Create todo'}
          </h2>
          {editingId && (
            <button type="button" className="ghost-button" onClick={resetForm}>
              Cancel
            </button>
          )}
        </div>

        <form className="todo-form" onSubmit={handleSubmit}>
          <label>
            Title
            <input
              name="title"
              value={form.title}
              onChange={updateForm}
              maxLength={200}
              required
            />
          </label>

          <label className="full-width">
            Description
            <textarea
              name="description"
              value={form.description}
              onChange={updateForm}
              maxLength={1000}
              rows="3"
            />
          </label>

          <label>
            Category
            <select name="category" value={form.category} onChange={updateForm}>
              {categories.map((category) => (
                <option key={category} value={category}>
                  {category}
                </option>
              ))}
            </select>
          </label>

          <label>
            Priority
            <select name="priority" value={form.priority} onChange={updateForm}>
              {priorities.map((priority) => (
                <option key={priority} value={priority}>
                  {priority}
                </option>
              ))}
            </select>
          </label>

          <button type="submit" className="primary-button" disabled={isSaving}>
            {isSaving ? 'Saving...' : editingId ? 'Save changes' : 'Create todo'}
          </button>
        </form>
      </section>

      {error && <div className="error-message">{error}</div>}

      <section className="list-section" aria-labelledby="todo-list-heading">
        <div className="section-heading">
          <h2 id="todo-list-heading">Todo list</h2>
          <span className="summary">
            {completedCount} of {todos.length} completed
          </span>
        </div>

        {isLoading ? (
          <p className="empty-state">Loading todos...</p>
        ) : todos.length === 0 ? (
          <p className="empty-state">No todos found.</p>
        ) : (
          <ul className="todo-list">
            {todos.map((todo) => (
              <li
                className={`todo-item ${todo.isCompleted ? 'is-completed' : ''}`}
                key={todo.id}
              >
                <label className="complete-control">
                  <input
                    type="checkbox"
                    checked={todo.isCompleted}
                    disabled={mutatingId === todo.id}
                    onChange={() => toggleCompleted(todo)}
                  />
                  <span className="sr-only">
                    Mark {todo.title} as complete
                  </span>
                </label>

                <div className="todo-copy">
                  <div className="todo-title-row">
                    <h3>{todo.title}</h3>
                    <span className={`priority-badge ${todo.priority.toLowerCase()}`}>
                      {todo.priority}
                    </span>
                  </div>

                  {todo.description && <p>{todo.description}</p>}

                  <div className="todo-meta">
                    <span>{todo.category}</span>
                    {todo.updatedAt && <span>Updated {formatDate(todo.updatedAt)}</span>}
                  </div>
                </div>

                <div className="todo-actions">
                  <button
                    type="button"
                    className="secondary-button"
                    onClick={() => startEditing(todo)}
                  >
                    Edit
                  </button>
                  <button
                    type="button"
                    className="danger-button"
                    disabled={mutatingId === todo.id}
                    onClick={() => removeTodo(todo)}
                  >
                    Delete
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </section>
    </main>
  )
}

function formatDate(value) {
  return new Date(value).toLocaleDateString(undefined, {
    month: 'short',
    day: 'numeric',
  })
}

export default App
