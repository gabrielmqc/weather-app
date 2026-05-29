<template>
  <div class="weather-card" :class="{ 'weather-card--detailed': detailed }">
    <div class="weather-card__header">
      <div class="city-info">
        <h3>
          <i class="pi pi-building"></i>
          {{ weather.city }}
        </h3>
        <div class="temperature">
          <i class="pi" :class="getTemperatureIcon(weather.temperature)"></i>
          {{ weather.temperature.toFixed(1) }}°C
        </div>
      </div>
      <div class="weather-icon">
        <i class="pi" :class="getTemperatureIcon(weather.temperature)"></i>
      </div>
    </div>

    <div class="weather-card__body">
      <div class="info-row">
        <span>
          <i class="pi pi-calendar"></i>
          Data/Hora:
        </span>
        <strong>{{ formatDate(weather.recordedAt) }}</strong>
      </div>
      <div class="info-row">
        <span>
          <i class="pi pi-arrow-right"></i>
          Latitude:
        </span>
        <strong>{{ weather.latitude.toFixed(4) }}</strong>
      </div>
      <div class="info-row">
        <span>
          <i class="pi pi-arrow-up"></i>
          Longitude:
        </span>
        <strong>{{ weather.longitude.toFixed(4) }}</strong>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { WeatherRecord } from '@/@types/WeatherRecordData'
import 'primeicons/primeicons.css'

defineProps<{
  weather: WeatherRecord
  detailed?: boolean
}>()

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
  })
}
</script>

<style scoped>
.weather-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.weather-card__header {
  background: #42b883;
  color: white;
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.weather-card--detailed .weather-card__header {
  padding: 2rem;
}

.city-info h3 {
  font-size: 1.5rem;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.temperature {
  font-size: 2rem;
  font-weight: bold;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.weather-card--detailed .temperature {
  font-size: 3rem;
}

.weather-icon {
  font-size: 3rem;
  opacity: 0.9;
}

.weather-card--detailed .weather-icon {
  font-size: 4rem;
}

.weather-card__body {
  padding: 1.5rem;
}

.weather-card--detailed .weather-card__body {
  padding: 2rem;
}

.info-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 0;
  border-bottom: 1px solid #f0f0f0;
}

.info-row:last-child {
  border-bottom: none;
}

.info-row span {
  color: #666;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.info-row span i {
  color: #42b883;
}

.info-row strong {
  color: #2c3e50;
}
</style>
