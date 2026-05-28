<template>
  <div class="weather-history">
    <h1>
      <i class="pi pi-history"></i>
      Histórico de Temperaturas
    </h1>

    <div class="search-section">
      <div class="search-form">
        <div class="form-group">
          <label>
            <i class="pi pi-building"></i>
            Buscar por Cidade:
          </label>
          <div class="input-group">
            <span class="input-icon">
              <i class="pi pi-search"></i>
            </span>
            <input
              v-model="searchCity"
              type="text"
              placeholder="Nome da cidade"
              @keyup.enter="handleSearchHistory"
            />
          </div>
        </div>

        <div class="divider">
          <span>ou</span>
        </div>

        <div class="form-group">
          <label>
            <i class="pi pi-map-marker"></i>
            Buscar por Coordenadas:
          </label>
          <div class="coordinates-inputs">
            <div class="input-group">
              <span class="input-icon">
                <i class="pi pi-arrow-right"></i>
              </span>
              <input v-model.number="searchLat" type="number" placeholder="Latitude" step="any" />
            </div>
            <div class="input-group">
              <span class="input-icon">
                <i class="pi pi-arrow-up"></i>
              </span>
              <input v-model.number="searchLon" type="number" placeholder="Longitude" step="any" />
            </div>
          </div>
        </div>

        <button @click="handleSearchHistory" :disabled="loading" class="btn-search">
          <i v-if="!loading" class="pi pi-search"></i>
          <i v-if="loading" class="pi pi-spin pi-spinner"></i>
          {{ loading ? 'Buscando...' : 'Buscar Histórico' }}
        </button>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="loading">
      <div class="loading-icon">
        <i class="pi pi-spin pi-spinner" style="font-size: 3rem; color: #42b883"></i>
      </div>
      <p>Carregando histórico...</p>
    </div>

    <!-- Tabela -->
    <div v-if="sortedHistory.length > 0 && !loading" class="history-content">
      <div class="table-header">
        <h2>
          <i class="pi pi-list"></i>
          {{ sortedHistory.length }} Registros Encontrados
        </h2>
        <button @click="toggleView" class="btn-toggle">
          <i v-if="!showChart" class="pi pi-chart-line"></i>
          <i v-else class="pi pi-table"></i>
          {{ showChart ? 'Ver Tabela' : 'Ver Gráfico' }}
        </button>
      </div>

      <!-- Gráfico -->
      <div v-if="showChart" class="chart-section">
        <canvas ref="chartCanvas"></canvas>
      </div>

      <!-- Tabela -->
      <div v-else class="table-section">
        <table>
          <thead>
            <tr>
              <th>
                <i class="pi pi-calendar"></i>
                Data/Hora
              </th>
              <th>
                <i class="pi pi-building"></i>
                Cidade
              </th>
              <th>
                <i class="pi pi-sun"></i>
                Temperatura
              </th>
              <th>
                <i class="pi pi-map-marker"></i>
                Coordenadas
              </th>
              <th>
                <i class="pi pi-cog"></i>
                Ações
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in sortedHistory" :key="record.id">
              <td>
                <i class="pi pi-clock"></i>
                {{ formatDate(record.recordedAt) }}
              </td>
              <td>{{ record.city }}</td>
              <td class="temperature-cell">
                <i class="pi" :class="getTemperatureIcon(record.temperature)"></i>
                {{ record.temperature.toFixed(1) }}°C
              </td>
              <td>{{ record.latitude.toFixed(4) }}, {{ record.longitude.toFixed(4) }}</td>
              <td>
                <router-link :to="`/weather/${record.id}`" class="btn-detail">
                  <i class="pi pi-info-circle"></i>
                  Detalhes
                </router-link>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div v-if="sortedHistory.length === 0 && !loading && searched" class="no-results">
      <div class="no-results-icon">
        <i class="pi pi-search" style="font-size: 4rem; color: #ccc"></i>
      </div>
      <h3>Nenhum registro encontrado</h3>
      <p>Não há registros de temperatura para esta localidade nos últimos 30 dias.</p>
      <router-link to="/weather" class="btn-primary">
        <i class="pi pi-plus"></i>
        Fazer Nova Consulta
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useWeather } from '@/composables/useWeather'
import { Chart, registerables } from 'chart.js'
import type { WeatherHistoryQuery } from '@/@types/WeatherRecordData'
import 'primeicons/primeicons.css'

Chart.register(...registerables)

const { sortedHistory, loading, fetchHistory } = useWeather()

const searchCity = ref('')
const searchLat = ref(0)
const searchLon = ref(0)
const searched = ref(false)
const showChart = ref(false)
const chartCanvas = ref<HTMLCanvasElement>()
let chart: Chart | null = null

function toggleView() {
  showChart.value = !showChart.value
}

function getTemperatureIcon(temperature: number): string {
  if (temperature >= 30) return 'pi-sun'
  if (temperature >= 25) return 'pi-sun'
  if (temperature >= 20) return 'pi-cloud-sun'
  if (temperature >= 15) return 'pi-cloud'
  if (temperature >= 10) return 'pi-cloud'
  return 'pi-snowflake'
}

function createChart() {
  if (!chartCanvas.value || !sortedHistory.value.length) return

  const ctx = chartCanvas.value.getContext('2d')
  if (!ctx) return

  if (chart) {
    chart.destroy()
  }

  const data = sortedHistory.value.sort(
    (a, b) => new Date(a.recordedAt).getTime() - new Date(b.recordedAt).getTime(),
  )

  chart = new Chart(ctx, {
    type: 'line',
    data: {
      labels: data.map((d) =>
        new Date(d.recordedAt).toLocaleDateString('pt-BR', {
          day: '2-digit',
          month: '2-digit',
          hour: '2-digit',
          minute: '2-digit',
        }),
      ),
      datasets: [
        {
          label: 'Temperatura (°C)',
          data: data.map((d) => d.temperature),
          borderColor: '#e74c3c',
          backgroundColor: 'rgba(231, 76, 60, 0.1)',
          tension: 0.4,
          fill: true,
          pointRadius: 4,
          pointHoverRadius: 6,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: true,
          position: 'top',
        },
        title: {
          display: true,
          text: `Histórico de Temperaturas - ${sortedHistory.value[0]?.city || ''}`,
          font: {
            size: 16,
          },
        },
      },
      scales: {
        y: {
          ticks: {
            callback: function (value) {
              return value + '°C'
            },
          },
        },
      },
    },
  })
}

watch(showChart, (newVal) => {
  if (newVal) {
    setTimeout(createChart, 100)
  }
})

async function handleSearchHistory() {
  const query: WeatherHistoryQuery = {}

  if (searchCity.value.trim()) {
    query.city = searchCity.value.trim()
  } else if (searchLat.value || searchLon.value) {
    query.lat = searchLat.value
    query.lon = searchLon.value
  } else {
    return
  }

  searched.value = true

  try {
    await fetchHistory(query)
    if (showChart.value) {
      setTimeout(createChart, 100)
    }
  } finally {
  }
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
.weather-history {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem 1rem;
}

h1 {
  color: #2c3e50;
  margin-bottom: 2rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

h1 i {
  color: #42b883;
}

.search-section {
  margin-bottom: 2rem;
}

.search-form {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  color: #333;
}

label i {
  color: #42b883;
}

.input-group {
  position: relative;
  display: flex;
  align-items: center;
}

.input-icon {
  position: absolute;
  left: 0.75rem;
  color: #999;
  z-index: 1;
}

.input-group input {
  padding-left: 2.25rem;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

input:focus {
  outline: none;
  border-color: #42b883;
}

.coordinates-inputs {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.5rem;
}

.divider {
  display: flex;
  align-items: center;
  text-align: center;
  margin: 1.5rem 0;
  color: #666;
}

.divider::before,
.divider::after {
  content: '';
  flex: 1;
  border-bottom: 1px solid #ddd;
}

.divider span {
  padding: 0 1rem;
}

.btn-search {
  width: 100%;
  padding: 0.75rem;
  background: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
  margin-top: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  transition: background 0.3s;
}

.btn-search:hover:not(:disabled) {
  background: #38a16f;
}

.btn-search:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.btn-toggle {
  padding: 0.5rem 1rem;
  background: #f0f0f0;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background 0.3s;
}

.btn-toggle:hover {
  background: #e0e0e0;
}

.loading {
  text-align: center;
  padding: 3rem;
}

.loading-icon {
  margin-bottom: 1rem;
}

.history-content {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  border-bottom: 1px solid #eee;
}

.table-header h2 {
  margin: 0;
  color: #2c3e50;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.table-header h2 i {
  color: #42b883;
}

.table-section {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}

th {
  background: #f8f9fa;
  font-weight: bold;
  color: #2c3e50;
  white-space: nowrap;
}

th i {
  color: #42b883;
}

tr {
  display: table-row;
}

.temperature-cell {
  font-weight: bold;
  color: #e74c3c;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

tr:hover {
  background: #f8f9fa;
}

.btn-detail {
  padding: 0.25rem 0.75rem;
  background: #42b883;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  font-size: 0.9rem;
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  transition: background 0.3s;
}

.btn-detail:hover {
  background: #38a16f;
}

.chart-section {
  padding: 1.5rem;
  height: 400px;
}

.no-results {
  text-align: center;
  padding: 3rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.no-results-icon {
  margin-bottom: 1rem;
}

.no-results h3 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.no-results p {
  color: #666;
  margin-bottom: 1.5rem;
}

.btn-primary {
  padding: 0.75rem 1.5rem;
  background: #42b883;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: background 0.3s;
}

.btn-primary:hover {
  background: #38a16f;
}
</style>
