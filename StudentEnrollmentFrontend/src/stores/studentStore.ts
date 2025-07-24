import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Student, StudentCreateDto, StudentUpdateDto } from '@/types/student'
import { studentService } from '@/services/studentService'

export const useStudentStore = defineStore('student', () => {
  const students = ref<Student[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const studentsCount = computed(() => students.value.length)

  async function fetchStudents() {
    loading.value = true
    error.value = null
    try {
      students.value = await studentService.getAll()
    } catch (err) {
      error.value = 'Erro ao carregar alunos'
      console.error('Error fetching students:', err)
    } finally {
      loading.value = false
    }
  }

  async function createStudent(studentData: StudentCreateDto) {
    loading.value = true
    error.value = null
    try {
      const newStudent = await studentService.create(studentData)
      students.value.push(newStudent)
      return newStudent
    } catch (err: any) {
      error.value = err.message || 'Erro ao criar aluno'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateStudent(id: number, studentData: StudentUpdateDto) {
    loading.value = true
    error.value = null
    try {
      const updatedStudent = await studentService.update(id, studentData)
      const index = students.value.findIndex(s => s.Id === id)
      if (index !== -1) {
        students.value[index] = updatedStudent
      }
      return updatedStudent
    } catch (err: any) {
      error.value = err.message || 'Erro ao atualizar aluno'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteStudent(id: number) {
    loading.value = true
    error.value = null
    try {
      await studentService.delete(id)
      students.value = students.value.filter(s => s.Id !== id)
    } catch (err: any) {
      error.value = err.message || 'Erro ao excluir aluno'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function getStudent(id: number): Promise<Student | null> {
    const student = students.value.find(s => s.Id === id)
    if (student) {
      return student
    }
    
    loading.value = true
    error.value = null
    try {
      return await studentService.getById(id)
    } catch (err: any) {
      error.value = err.message || 'Erro ao buscar aluno'
      return null
    } finally {
      loading.value = false
    }
  }

  return {
    students,
    loading,
    error,
    studentsCount,
    fetchStudents,
    createStudent,
    updateStudent,
    deleteStudent,
    getStudent,
  }
})
