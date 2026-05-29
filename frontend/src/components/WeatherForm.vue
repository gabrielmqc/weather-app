<template>
  <div class="weather-form">
    <h2>Consultar Clima</h2>

    <div class="form-tabs">
      <button :class="{ active: searchType === 'city' }" @click="searchType = 'city'">
        Por Cidade
      </button>
      <button :class="{ active: searchType === 'coordinates' }" @click="searchType = 'coordinates'">
        Por Coordenadas
      </button>
    </div>

    <div v-if="searchType === 'city'" class="form-group">
      <label for="city">Nome da Cidade:</label>
      <input
        id="city"
        v-model="cityName"
        type="text"
        placeholder="Ex: São Paulo"
        @keyup.enter="searchByCity"
      />
      <button @click="searchByCity" :disabled="loading">
        {{ loading ? 'Buscando...' : 'Buscar' }}
      </button>
    </div>

    <div v-if="searchType === 'coordinates'" class="form-group">
      <label for="latitude">Latitude:</label>
      <input
        id="latitude"
        v-model.number="latitude"
        type="number"
        step="any"
        placeholder="Ex: -23.5505"
      />

      <label for="longitude">Longitude:</label>
      <input
        id="longitude"
        v-model.number="longitude"
        type="number"
        step="any"
        placeholder="Ex: -46.6333"
      />

      <button @click="searchByCoordinates" :disabled="loading">
        {{ loading ? 'Buscando...' : 'Buscar' }}
      </button>
    </div>

    <div v-if="currentWeather" class="current-weather">
      <h3>Resultado Atual</h3>
      <div class="weather-card">
        <div class="city-name">{{ currentWeather.city }}</div>
        <div class="temperature">{{ currentWeather.temperature }}°C</div>
        <div class="coordinates">
          Lat: {{ currentWeather.latitude }} | Lon: {{ currentWeather.longitude }}
        </div>
        <div class="timestamp">Registrado em: {{ formatDate(currentWeather.recordedAt) }}</div>
      </div>
    </div>

    <div v-if="error" class="error-message">
      {{ error }}
    </div>
  </div>
</template>

<script setup lang="ts">
import type { CoordinatesDataToSend } from '@/@types/CoordinatesData'
import type { WeatherRecord } from '@/@types/WeatherRecordData'
import { weatherService } from '@/services/weatherService'
import { ref } from 'vue'

const emit = defineEmits<{
  weatherUpdated: [weather: WeatherRecord]
}>()

const searchType = ref<'city' | 'coordinates'>('city')
const cityName = ref('')
const latitude = ref(0)
const longitude = ref(0)
const loading = ref(false)
const error = ref('')
const currentWeather = ref<WeatherRecord | null>(null)

async function searchByCity() {
  if (!cityName.value.trim()) {
    error.value = 'Por favor, insira o nome da cidade'
    return
  }

  loading.value = true
  error.value = ''

  try {
    currentWeather.value = await weatherService.getByCity(cityName.value)
    emit('weatherUpdated', currentWeather.value)
  } catch (err: unknown) {
    error.value = err instanceof Error ? err.message : 'Erro ao buscar dados do clima'
  } finally {
    loading.value = false
  }
}

async function searchByCoordinates() {
  if (!latitude.value || !longitude.value) {
    error.value = 'Por favor, insira latitude e longitude'
    return
  }

  loading.value = true
  error.value = ''

  try {
    currentWeather.value = await weatherService.getByCoordinates({
      lat: latitude.value,
      lon: longitude.value,
    } as CoordinatesDataToSend)
    emit('weatherUpdated', currentWeather.value)
  } catch (err: unknown) {
    error.value = err instanceof Error ? err.message : 'Erro ao buscar dados do clima'
  } finally {
    loading.value = false
  }
}

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString('pt-BR')
}
</script>

<style scoped>
.weather-form {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.form-tabs {
  display: flex;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.form-tabs button {
  padding: 0.5rem 1rem;
  border: none;
  background: #f0f0f0;
  cursor: pointer;
  border-radius: 4px;
}

.form-tabs button.active {
  background: #42b883;
  color: white;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

label {
  font-weight: bold;
  margin-bottom: 0.25rem;
}

input {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

button {
  padding: 0.75rem;
  background: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  margin-top: 0.5rem;
}

button:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.current-weather {
  margin-top: 2rem;
}

.weather-card {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  border-radius: 8px;
  margin-top: 1rem;
}

.city-name {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.temperature {
  font-size: 3rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.coordinates {
  font-size: 0.9rem;
  opacity: 0.9;
  margin-bottom: 0.5rem;
}

.timestamp {
  font-size: 0.8rem;
  opacity: 0.8;
}

.error-message {
  background: #fee;
  color: #c33;
  padding: 1rem;
  border-radius: 4px;
  margin-top: 1rem;
}
</style>
