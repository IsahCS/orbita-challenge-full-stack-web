<template>
  <v-app>
    <v-navigation-drawer
      v-model="drawer"
      app
      permanent
      width="250"
      color="grey-lighten-4"
    >
      <v-list>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title class="text-h6 mb-2">
              <v-img
                src="/grupo-a-logo.png"
                alt="Grupo A"
                max-height="40"
                contain
                class="mb-2"
              />
            </v-list-item-title>
            <v-list-item-subtitle class="text-h6 font-weight-bold">
              Módulo Acadêmico
            </v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>

        <v-divider class="my-3"></v-divider>

        <v-list-item
          to="/students"
          color="primary"
          class="mb-2"
        >
          <template v-slot:prepend>
            <v-icon>mdi-account-group</v-icon>
          </template>
          <v-list-item-title>Alunos</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app color="white" elevation="1">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const drawer = ref(true)
</script>

<style scoped>
.v-navigation-drawer {
  border-right: 1px solid #e0e0e0;
}
</style>
