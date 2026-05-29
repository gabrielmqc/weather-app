<!-- views/WeatherHistory.vue -->
<template>
  <div class="weather-history">
    <h1>
      <i class="pi pi-history"></i>
      Histórico de Temperaturas
    </h1>

    <WeatherSearchForm @search="handleSearch" />

    <WeatherHistoryLoading v-if="loading" />

    <WeatherHistoryEmpty v-if="!loading && searched && sortedHistory.length === 0" />

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

      <WeatherHistoryChart v-if="showChart" :history="sortedHistory" />

      <WeatherHistoryTable v-else :history="sortedHistory" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useWeather } from '@/composables/useWeather'
import type { WeatherHistoryQuery } from '@/@types/WeatherRecordData'
import WeatherSearchForm from '@/components/history/WeatherSearchForm.vue'
import WeatherHistoryTable from '@/components/history/WeatherHistoryTable.vue'
import WeatherHistoryChart from '@/components/history/WeatherHistoryChart.vue'
import WeatherHistoryEmpty from '@/components/history/WeatherHistoryEmpty.vue'
import WeatherHistoryLoading from '@/components/history/WeatherHistoryLoading.vue'
import 'primeicons/primeicons.css'

const { sortedHistory, loading, fetchHistory } = useWeather()

const searched = ref(false)
const showChart = ref(false)

function toggleView() {
  showChart.value = !showChart.value
}

async function handleSearch(query: WeatherHistoryQuery) {
  searched.value = true
  try {
    await fetchHistory(query)
  } catch (error) {
    console.error('Erro ao buscar histórico:', error)
  }
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
</style>
