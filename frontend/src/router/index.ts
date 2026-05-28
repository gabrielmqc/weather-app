import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/weather',
      name: 'weather-search',
      component: () => import('@/views/WeatherSearchView.vue'),
    },
    {
      path: '/weather/history',
      name: 'weather-history',
      component: () => import('@/views/WeatherHistoryView.vue'),
    },
    {
      path: '/weather/:id',
      name: 'weather-detail',
      component: () => import('@/views/WeatherDetailView.vue'),
    },
  ],
})

export default router
