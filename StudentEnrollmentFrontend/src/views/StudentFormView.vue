<template>
  <v-container fluid class="pa-6">
    <v-card>
      <v-card-title class="text-h4 pa-6 bg-grey-lighten-4">
        {{ isEditing ? 'Editar Aluno' : 'Cadastro de aluno' }}
      </v-card-title>
      <v-card-text class="pa-6">
        <v-form ref="form" v-model="valid" @submit.prevent="handleSubmit">
          <v-row>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="formData.name"
                label="Nome"
                placeholder="Informe o nome completo"
                variant="outlined"
                :rules="nameRules"
                required
                class="mb-4"
              ></v-text-field>
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="formData.email"
                label="E-mail"
                placeholder="Informe apenas um e-mail"
                variant="outlined"
                type="email"
                :rules="emailRules"
                required
                class="mb-4"
              ></v-text-field>
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="formData.ra"
                label="RA"
                placeholder="Informe o registro acadêmico"
                variant="outlined"
                :rules="raRules"
                :readonly="isEditing"
                :disabled="isEditing"
                :required="!isEditing"
                class="mb-4"
              ></v-text-field>
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="formData.cpf"
                label="CPF"
                placeholder="Informe o número do documento"
                variant="outlined"
                :rules="cpfRules"
                :readonly="isEditing"
                :disabled="isEditing"
                :required="!isEditing"
                class="mb-4"
                @input="formatCPF"
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row class="mt-4">
            <v-spacer></v-spacer>
            <v-col cols="auto">
              <v-btn
                color="grey"
                variant="flat"
                size="large"
                @click="handleCancel"
                class="mr-4"
              >
                Cancelar
              </v-btn>
            </v-col>
            <v-col cols="auto">
              <v-btn
                color="grey"
                variant="flat"
                size="large"
                type="submit"
                :loading="loading"
                :disabled="!valid"
              >
                Salvar
              </v-btn>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
    <v-snackbar
      v-model="snackbar"
      :color="snackbarColor"
      top
      timeout="4000"
    >
      {{ snackbarMessage }}
      <template v-slot:actions>
        <v-btn
          color="white"
          variant="text"
          @click="snackbar = false"
        >
          Fechar
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch, nextTick } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useStudentStore } from '../stores/studentStore'
import type { StudentCreateDto, StudentUpdateDto } from '../types/student'

const router = useRouter()
const route = useRoute()
const studentStore = useStudentStore()

//determina se estamos editando um aluno existente
const studentId = computed(() => {
  const id = route.params.id
  return id ? Number(id) : null
})

const isEditing = computed(() => !!studentId.value)

const valid = ref(false)
const form = ref()
const snackbar = ref(false)
const snackbarMessage = ref('')
const snackbarColor = ref('success')

const formData = ref({
  name: '',
  email: '',
  ra: '',
  cpf: ''
})

const loading = computed(() => studentStore.loading)

const nameRules = [
  (v: string) => !!v || 'Nome é obrigatório',
  (v: string) => (v && v.length >= 2) || 'Nome deve ter pelo menos 2 caracteres',
  (v: string) => (v && v.length <= 100) || 'Nome deve ter no máximo 100 caracteres',
]

const emailRules = [
  (v: string) => !!v || 'E-mail é obrigatório',
  (v: string) => /.+@.+\..+/.test(v) || 'E-mail deve ser válido',
]

const raRules = computed(() => [
  (v: string) => !isEditing.value ? (!!v || 'RA é obrigatório') : true,
  (v: string) => !isEditing.value ? ((v && v.length <= 20) || 'RA deve ter no máximo 20 caracteres') : true,
])

const cpfRules = computed(() => [
  (v: string) => !isEditing.value ? (!!v || 'CPF é obrigatório') : true,
  (v: string) => {
    if (isEditing.value) return true
    const cpf = v.replace(/\D/g, '')
    return cpf.length === 11 || 'CPF deve ter 11 dígitos'
  },
  (v: string) => {
    if (isEditing.value) return true
    const cpf = v.replace(/\D/g, '')
    return validateCPF(cpf) || 'CPF inválido'
  }
])

function formatCPF(event: any) {
  let value = event.target.value.replace(/\D/g, '')
  if (value.length <= 11) {
    value = value.replace(/(\d{3})(\d)/, '$1.$2')
    value = value.replace(/(\d{3})(\d)/, '$1.$2')
    value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2')
    formData.value.cpf = value
  }
}

function validateCPF(cpf: string): boolean {
  if (cpf.length !== 11 || /^(\d)\1{10}$/.test(cpf)) {
    return false
  }

  let sum = 0
  for (let i = 0; i < 9; i++) {
    sum += parseInt(cpf.charAt(i)) * (10 - i)
  }
  
  let remainder = (sum * 10) % 11
  if (remainder === 10 || remainder === 11) remainder = 0
  if (remainder !== parseInt(cpf.charAt(9))) return false

  sum = 0
  for (let i = 0; i < 10; i++) {
    sum += parseInt(cpf.charAt(i)) * (11 - i)
  }
  
  remainder = (sum * 10) % 11
  if (remainder === 10 || remainder === 11) remainder = 0
  if (remainder !== parseInt(cpf.charAt(10))) return false

  return true
}

async function handleSubmit() {
  if (!valid.value) return

  try {
    const cpfNumbers = formData.value.cpf.replace(/\D/g, '')
    
    if (isEditing.value && studentId.value) {
      const updateData: StudentUpdateDto = {
        name: formData.value.name,
        email: formData.value.email
      }
      await studentStore.updateStudent(studentId.value, updateData)
      showSnackbar('Aluno atualizado com sucesso!', 'success')
    } else {
      const createData: StudentCreateDto = {
        name: formData.value.name,
        email: formData.value.email,
        ra: formData.value.ra,
        cpf: cpfNumbers
      }
      await studentStore.createStudent(createData)
      showSnackbar('Aluno criado com sucesso!', 'success')
    }
    
    setTimeout(() => {
      router.push('/students')
    }, 1000)
  } catch (error: any) {
    showSnackbar(error.message || 'Erro ao salvar aluno', 'error')
  }
}

function handleCancel() {
  router.push('/students')
}

function showSnackbar(message: string, color: string = 'success') {
  snackbarMessage.value = message
  snackbarColor.value = color
  snackbar.value = true
}

//força a validação do formulário
async function forceValidation() {
  await nextTick()
  if (form.value) {
    form.value.resetValidation()
    await nextTick()
    const isValid = await form.value.validate()
    valid.value = isValid.valid
    return isValid.valid
  }
  return false
}

async function loadStudent() {
  if (!studentId.value) return

  try {
    const student = await studentStore.getStudent(studentId.value)
    if (student) {
      formData.value = {
        name: student.Name,
        email: student.Email,
        ra: student.RA,
        cpf: student.CPF
      }
      
      //força a validação do formulário após carregar os dados
      setTimeout(async () => {
        await forceValidation()
      }, 100)
    } else {
      showSnackbar('Aluno não encontrado', 'error')
      router.push('/students')
    }
  } catch (error) {
    showSnackbar('Erro ao carregar dados do aluno', 'error')
    router.push('/students')
  }
}

//escuta mudanças no id do aluno
watch(studentId, async () => {
  if (isEditing.value) {
    await loadStudent()
  } else {
    formData.value = {
      name: '',
      email: '',
      ra: '',
      cpf: ''
    }
    //aguarda um tick e reseta a validação
    await nextTick()
    if (form.value) {
      form.value.resetValidation()
    }
  }
})

//escuta mudanças nos dados do formulário
watch(formData, async () => {
  await nextTick()
  if (form.value) {
    form.value.validate()
  }
}, { deep: true, immediate: true })

//escuta mudanças em campos específicos que podem afetar a validação
watch([() => formData.value.name, () => formData.value.email], async () => {
  await nextTick()
  if (form.value && isEditing.value) {
    form.value.validate()
  }
})

//lifecycle hook para carregar os dados do aluno ao montar o componente
onMounted(async () => {
  if (isEditing.value) {
    await loadStudent()
  } else {
    //para novos alunos, aguarda um tick e reseta a validação
    await nextTick()
    if (form.value) {
      form.value.resetValidation()
    }
  }
})
</script>

<style scoped>
.v-card-title {
  border-bottom: 1px solid #e0e0e0;
}

.v-text-field--disabled {
  opacity: 0.6;
}
</style>
