<template>
  <v-container fluid class="pa-6">
    <v-card>
      <v-card-title class="text-h4 pa-6 bg-grey-lighten-4">
        Consulta de alunos
      </v-card-title>

      <v-card-text class="pa-6">
        <!-- Barra de pesquisa e botão cadastrar -->
        <v-row class="mb-4" align="center">
          <v-col cols="12" md="8">
            <v-text-field
              v-model="searchQuery"
              placeholder="Digite sua busca"
              variant="outlined"
              density="compact"
              append-inner-icon="mdi-magnify"
              hide-details
              @keyup.enter="performSearch"
            >
              <template v-slot:append>
                <v-btn
                  color="grey"
                  variant="flat"
                  size="large"
                  @click="performSearch"
                >
                  Pesquisar
                </v-btn>
              </template>
            </v-text-field>
          </v-col>
          <v-col cols="12" md="4" class="text-right">
            <v-btn
              color="grey"
              variant="flat"
              size="large"
              to="/students/new"
            >
              Cadastrar Aluno
            </v-btn>
          </v-col>
        </v-row>

        <!-- Tabela de alunos -->
        <v-data-table
          :headers="headers"
          :items="filteredStudents"
          :loading="loading"
          class="elevation-1"
          no-data-text="Nenhum aluno encontrado"
          loading-text="Carregando alunos..."
        >
          <template v-slot:[`item.actions`]="{ item }">
            <v-tooltip text="Editar" location="top">
              <template v-slot:activator="{ props }">
                <v-btn
                  v-bind="props"
                  icon="mdi-pencil"
                  color="primary"
                  variant="text"
                  size="small"
                  :to="`/students/${item.Id}/edit`"
                  class="mr-1"
                ></v-btn>
              </template>
            </v-tooltip>
            
            <v-tooltip text="Excluir" location="top">
              <template v-slot:activator="{ props }">
                <v-btn
                  v-bind="props"
                  icon="mdi-delete"
                  color="error"
                  variant="text"
                  size="small"
                  @click="openDeleteDialog(item)"
                ></v-btn>
              </template>
            </v-tooltip>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>

    <!-- Dialog de confirmação de exclusão -->
    <v-dialog v-model="deleteDialog" max-width="500px">
      <v-card>
        <v-card-title class="text-h5">
          Confirmar Exclusão
        </v-card-title>
        <v-card-text>
          Tem certeza que deseja excluir o aluno <strong>{{ studentToDelete?.Name }}</strong>?
          Esta ação não pode ser desfeita.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            variant="text"
            @click="closeDeleteDialog"
          >
            Cancelar
          </v-btn>
          <v-btn
            color="error"
            variant="flat"
            @click="confirmDelete"
            :loading="loading"
          >
            Confirmar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar para mensagens -->
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
import { ref, computed, onMounted } from 'vue'
import { useStudentStore } from '../stores/studentStore'
import type { Student } from '../types/student'

const studentStore = useStudentStore()

const searchQuery = ref('')
const deleteDialog = ref(false)
const studentToDelete = ref<Student | null>(null)
const snackbar = ref(false)
const snackbarMessage = ref('')
const snackbarColor = ref('success')

const loading = computed(() => studentStore.loading)
const students = computed(() => studentStore.students)

const filteredStudents = computed(() => {
  if (!searchQuery.value) {
    return students.value
  }
  
  const query = searchQuery.value.toLowerCase()
  return students.value.filter(student => 
    student.Name.toLowerCase().includes(query) ||
    student.Email.toLowerCase().includes(query) ||
    student.RA.toLowerCase().includes(query) ||
    student.CPF.includes(query)
  )
})

const headers = [
  { title: 'Registro Acadêmico', key: 'RA', sortable: true },
  { title: 'Nome', key: 'Name', sortable: true },
  { title: 'CPF', key: 'CPF', sortable: true },
  { title: 'Ações', key: 'actions', sortable: false },
]

function performSearch() {
  //a busca é feita automaticamente pelo computed
}

function openDeleteDialog(student: Student) {
  studentToDelete.value = student
  deleteDialog.value = true
}

function closeDeleteDialog() {
  deleteDialog.value = false
  studentToDelete.value = null
}

async function confirmDelete() {
  if (!studentToDelete.value) return
  
  try {
    await studentStore.deleteStudent(studentToDelete.value.Id)
    showSnackbar('Aluno excluído com sucesso!', 'success')
    closeDeleteDialog()
  } catch (error) {
    showSnackbar('Erro ao excluir aluno', 'error')
  }
}

function showSnackbar(message: string, color: string = 'success') {
  snackbarMessage.value = message
  snackbarColor.value = color
  snackbar.value = true
}

onMounted(async () => {
  await studentStore.fetchStudents()
})
</script>

<style scoped>
.v-data-table {
  background-color: white;
}

.v-card-title {
  border-bottom: 1px solid #e0e0e0;
}
</style>
