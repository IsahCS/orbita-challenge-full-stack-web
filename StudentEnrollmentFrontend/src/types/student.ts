export interface Student {
  Id: number
  Name: string
  Email: string
  RA: string
  CPF: string
  CreatedAt: string
  UpdatedAt: string
}

export interface StudentCreateDto {
  name: string
  email: string
  ra: string
  cpf: string
}

export interface StudentUpdateDto {
  name: string
  email: string
}

export interface ApiResponse<T = any> {
  success: boolean
  message?: string
  data?: T
  errors?: string[]
}
