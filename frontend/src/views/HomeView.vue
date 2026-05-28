<template>
  <div class="home">
    <div class="hero">
      <div class="hero-icon">
        <i class="pi pi-cloud" style="font-size: 4rem; color: #42b883"></i>
      </div>
      <h1>Weather App</h1>
      <p>Sistema de Monitoramento de Clima</p>
    </div>

    <div class="actions">
      <router-link to="/weather" class="action-card">
        <div class="card-icon">
          <i class="pi pi-search"></i>
        </div>
        <h2>Consultar Clima</h2>
        <p>Busque a temperatura atual por cidade ou coordenadas</p>
      </router-link>

      <router-link to="/weather/history" class="action-card">
        <div class="card-icon">
          <i class="pi pi-chart-line"></i>
        </div>
        <h2>Histórico</h2>
        <p>Visualize o histórico de temperaturas dos últimos 30 dias</p>
      </router-link>
    </div>

    <div v-if="currentWeather" class="quick-view">
      <h2>
        <i class="pi pi-clock"></i>
        Última Consulta
      </h2>
      <WeatherCard :weather="currentWeather" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useWeatherStore } from '@/stores/weather'
import WeatherCard from '@/components/WeatherCard.vue'
import 'primeicons/primeicons.css'

const store = useWeatherStore()
const currentWeather = computed(() => store.currentWeather)
</script>

<style scoped>
.home {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

.hero {
  text-align: center;
  margin-bottom: 3rem;
}

.hero-icon {
  margin-bottom: 1.5rem;
}

.hero h1 {
  font-size: 3rem;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.hero p {
  font-size: 1.2rem;
  color: #666;
}

.actions {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
  margin-bottom: 3rem;
}

.action-card {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  text-decoration: none;
  color: inherit;
  transition:
    transform 0.3s,
    box-shadow 0.3s;
  border-top: 4px solid transparent;
}

.action-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
  border-top-color: #42b883;
}

.card-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  color: #42b883;
}

.action-card h2 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.action-card p {
  color: #666;
}

.quick-view {
  margin-top: 3rem;
}

.quick-view h2 {
  margin-bottom: 1rem;
  color: #2c3e50;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.quick-view h2 i {
  color: #42b883;
}
</style>
