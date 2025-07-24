import axios from 'axios'
import type { Student, StudentCreateDto, StudentUpdateDto, ApiResponse } from '@/types/student'

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

export const studentService = {
  async getAll(): Promise<Student[]> {
    const response = await api.get<ApiResponse<Student[]>>('/students')
    return response.data.data || []
  },

  async getById(id: number): Promise<Student | null> {
    try {
      const response = await api.get<ApiResponse<Student>>(`/students/${id}`)
      return response.data.data || null
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.status === 404) {
        return null
      }
      throw error
    }
  },

  async create(student: StudentCreateDto): Promise<Student> {
    try {
      const response = await api.post<ApiResponse<Student>>('/students', student)
      if (!response.data.success || !response.data.data) {
        throw new Error(response.data.message || 'Erro ao criar aluno')
      }
      return response.data.data
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.data?.message) {
          throw new Error(error.response.data.message)
        }
        if (error.response?.data?.errors) {
          const validationErrors = error.response.data.errors
          const firstErrorKey = Object.keys(validationErrors)[0]
          const firstError = validationErrors[firstErrorKey][0]
          throw new Error(firstError)
        }
        if (error.response?.data?.title) {
          throw new Error(error.response.data.title)
        }
      }
      throw error
    }
  },

  async update(id: number, student: StudentUpdateDto): Promise<Student> {
    try {
      const response = await api.put<ApiResponse<Student>>(`/students/${id}`, student)
      if (!response.data.success || !response.data.data) {
        throw new Error(response.data.message || 'Erro ao atualizar aluno')
      }
      return response.data.data
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.data?.message) {
          throw new Error(error.response.data.message)
        }
        if (error.response?.data?.errors) {
          const validationErrors = error.response.data.errors
          const firstErrorKey = Object.keys(validationErrors)[0]
          const firstError = validationErrors[firstErrorKey][0]
          throw new Error(firstError)
        }
        if (error.response?.data?.title) {
          throw new Error(error.response.data.title)
        }
      }
      throw error
    }
  },

  async delete(id: number): Promise<void> {
    try {
      const response = await api.delete<ApiResponse>(`/students/${id}`)
      if (!response.data.success) {
        throw new Error(response.data.message || 'Erro ao excluir aluno')
      }
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.data?.message) {
        throw new Error(error.response.data.message)
      }
      throw error
    }
  },
}
