<template>
  <div class="weather-history">
    <h2>Histórico de Temperaturas</h2>

    <div class="search-history">
      <div class="form-group">
        <label>Buscar por Cidade:</label>
        <div class="search-input">
          <input
            v-model="searchCity"
            type="text"
            placeholder="Nome da cidade"
            @keyup.enter="searchHistory"
          />
        </div>
      </div>

      <div class="form-group">
        <label>Ou por Coordenadas:</label>
        <div class="coordinates-inputs">
          <input v-model.number="searchLat" type="number" placeholder="Latitude" step="any" />
          <input v-model.number="searchLon" type="number" placeholder="Longitude" step="any" />
        </div>
      </div>

      <button @click="searchHistory" :disabled="loading">
        {{ loading ? 'Buscando...' : 'Buscar Histórico' }}
      </button>
    </div>

    <div v-if="history.length > 0" class="history-list">
      <h3>Resultados ({{ history.length }} registros)</h3>
      <div class="table-responsive">
        <table>
          <thead>
            <tr>
              <th>Data/Hora</th>
              <th>Cidade</th>
              <th>Temperatura</th>
              <th>Coordenadas</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in history" :key="record.id">
              <td>{{ formatDate(record.recordedAt) }}</td>
              <td>{{ record.city }}</td>
              <td class="temperature-cell">{{ record.temperature }}°C</td>
              <td>{{ record.latitude.toFixed(4) }}, {{ record.longitude.toFixed(4) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="history.length === 0 && !loading && searched" class="no-results">
      Nenhum registro encontrado para esta localidade nos últimos 30 dias.
    </div>
  </div>
</template>

<script setup lang="ts">
import type { WeatherHistoryQuery, WeatherRecord } from '@/@types/WeatherRecordData'
import { weatherService } from '@/services/weatherService'
import { ref } from 'vue'

const searchCity = ref('')
const searchLat = ref(0)
const searchLon = ref(0)
const loading = ref(false)
const searched = ref(false)
const history = ref<WeatherRecord[]>([])

async function searchHistory() {
  loading.value = true
  searched.value = true

  try {
    const query: WeatherHistoryQuery = {}

    if (searchCity.value.trim()) {
      query.city = searchCity.value
    } else if (searchLat.value || searchLon.value) {
      query.lat = searchLat.value
      query.lon = searchLon.value
    } else {
      return
    }

    history.value = await weatherService.getHistory(query)
  } catch (error) {
    console.error('Erro ao buscar histórico:', error)
    history.value = []
  } finally {
    loading.value = false
  }
}

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString('pt-BR')
}

defineExpose({
  searchHistory,
})
</script>

<style scoped>
.weather-history {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.search-history {
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

.coordinates-inputs {
  display: flex;
  gap: 0.5rem;
}

input {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  width: 100%;
}

button {
  padding: 0.75rem 1.5rem;
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

.table-responsive {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background: #f8f9fa;
  font-weight: bold;
}

.temperature-cell {
  font-weight: bold;
  color: #e74c3c;
}

tr:hover {
  background: #f8f9fa;
}

.no-results {
  text-align: center;
  padding: 2rem;
  color: #666;
  font-style: italic;
}
</style>
