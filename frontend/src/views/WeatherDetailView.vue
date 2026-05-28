<template>
  <div class="weather-detail">
    <router-link to="/weather/history" class="back-link">
      <i class="pi pi-arrow-left"></i>
      Voltar para o Histórico
    </router-link>

    <div v-if="currentWeather" class="detail-content">
      <h1>
        <i class="pi pi-info-circle"></i>
        Detalhes do Registro
      </h1>

      <div class="detail-card">
        <div class="main-info">
          <div class="city-info">
            <h2>
              <i class="pi pi-building"></i>
              {{ currentWeather.city }}
            </h2>
            <div class="temperature-display">
              <i class="pi" :class="getTemperatureIcon(currentWeather.temperature)"></i>
              {{ currentWeather.temperature.toFixed(1) }}°C
            </div>
          </div>
          <div class="weather-icon">
            <i class="pi" :class="getTemperatureIcon(currentWeather.temperature)"></i>
          </div>
        </div>

        <div class="detail-grid">
          <div class="detail-item">
            <span class="label">
              <i class="pi pi-calendar"></i>
              Data/Hora:
            </span>
            <span class="value">{{ formatDate(currentWeather.recordedAt) }}</span>
          </div>
          <div class="detail-item">
            <span class="label">
              <i class="pi pi-arrow-right"></i>
              Latitude:
            </span>
            <span class="value">{{ currentWeather.latitude }}</span>
          </div>
          <div class="detail-item">
            <span class="label">
              <i class="pi pi-arrow-up"></i>
              Longitude:
            </span>
            <span class="value">{{ currentWeather.longitude }}</span>
          </div>
          <div class="detail-item">
            <span class="label">
              <i class="pi pi-id-card"></i>
              ID do Registro:
            </span>
            <span class="value">{{ currentWeather.id }}</span>
          </div>
        </div>
      </div>

      <div class="actions">
        <router-link to="/weather" class="btn-primary">
          <i class="pi pi-plus"></i>
          Nova Consulta
        </router-link>
        <router-link to="/weather/history" class="btn-secondary">
          <i class="pi pi-list"></i>
          Ver Histórico Completo
        </router-link>
      </div>
    </div>

    <div v-else class="not-found">
      <div class="not-found-icon">
        <i class="pi pi-exclamation-circle" style="font-size: 4rem; color: #ccc"></i>
      </div>
      <h2>Registro não encontrado</h2>
      <p>O registro solicitado não foi encontrado ou pode ter expirado.</p>
      <router-link to="/weather" class="btn-primary">
        <i class="pi pi-search"></i>
        Fazer Nova Consulta
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useWeatherStore } from '@/stores/weather'
import 'primeicons/primeicons.css'

const route = useRoute()
const store = useWeatherStore()

const currentWeather = computed(() => {
  return store.weatherHistory.find((w) => w.id === route.params.id) || store.currentWeather
})

function getTemperatureIcon(temperature: number): string {
  if (temperature >= 30) return 'pi-sun'
  if (temperature >= 25) return 'pi-sun'
  if (temperature >= 20) return 'pi-cloud-sun'
  if (temperature >= 15) return 'pi-cloud'
  if (temperature >= 10) return 'pi-cloud'
  return 'pi-snowflake'
}

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
  })
}
</script>

<style scoped>
.weather-detail {
  max-width: 860px;
  margin: 0 auto;
  padding: 2rem 1rem;
  background: #f6f7fb;
  min-height: 100vh;
}

.back-link {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  color: #42b883;
  text-decoration: none;
  margin-bottom: 1.5rem;
  font-weight: 500;
  font-size: 0.95rem;
}

.back-link:hover {
  opacity: 0.8;
}

.detail-content h1 {
  font-size: 1.5rem;
  font-weight: 600;
  color: #111827;
  margin-bottom: 1.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.detail-content h1 i {
  color: #4f46e5;
}

.detail-card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.04);
}

.main-info {
  background: #42b883;
  color: white;
  padding: 3rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.city-info h2 {
  font-size: 1.4rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.temperature-display {
  font-size: 3rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  letter-spacing: -1px;
}

.temperature-display i {
  font-size: 2rem;
  opacity: 0.9;
}

.weather-icon {
  font-size: 3.5rem;
  opacity: 0.8;
}

.detail-grid {
  padding: 1.5rem 2rem;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
  gap: 1rem 1.5rem;
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.label {
  font-size: 0.75rem;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  display: flex;
  align-items: center;
  gap: 0.4rem;
}

.label i {
  color: #9ca3af;
}

.value {
  font-size: 1rem;
  color: #111827;
  font-weight: 500;
  padding-left: 1.2rem;
}

.actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 1.5rem;
  flex-wrap: wrap;
}

.btn-primary {
  padding: 0.65rem 1.2rem;
  background: #42b883;
  color: white;
  border-radius: 10px;
  font-weight: 500;
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: 0.2s ease;
}

.btn-primary:hover {
  background: #41b782;
}

.btn-secondary {
  padding: 0.65rem 1.2rem;
  background: #ffffff;
  color: #4f46e5;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  font-weight: 500;
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: 0.2s ease;
}

.btn-secondary:hover {
  background: #f9fafb;
}

.not-found {
  text-align: center;
  padding: 4rem 2rem;
}

.not-found h2 {
  color: #111827;
  margin-bottom: 0.5rem;
}

.not-found p {
  color: #6b7280;
}
</style>
