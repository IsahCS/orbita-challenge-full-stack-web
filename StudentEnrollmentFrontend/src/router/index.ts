import { createRouter, createWebHistory } from 'vue-router'
import StudentsListView from '@/views/StudentsListView.vue'
import StudentFormView from '@/views/StudentFormView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/students'
    },
    {
      path: '/students',
      name: 'students',
      component: StudentsListView
    },
    {
      path: '/students/new',
      name: 'student-create',
      component: StudentFormView
    },
    {
      path: '/students/:id/edit',
      name: 'student-edit',
      component: StudentFormView,
      props: true
    }
  ]
})

export default router
