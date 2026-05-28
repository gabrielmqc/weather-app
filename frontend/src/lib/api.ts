import axios, { AxiosError } from 'axios'

interface ApiError {
  error: string
  statusCode: number
}

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    if (error.response?.data) {
      const apiError = error.response.data
      const message = apiError.error || 'Erro desconhecido'
      throw new Error(message)
    }

    if (error.code === 'ERR_NETWORK') {
      throw new Error('Erro de conexão. Verifique se o servidor está rodando.')
    }

    throw error
  },
)
