<template>
  <div class="table-section">
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
        <tr v-for="record in history" :key="record.id">
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
</template>

<script setup lang="ts">
import type { WeatherRecord } from '@/@types/WeatherRecordData'

defineProps<{
  history: WeatherRecord[]
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
</style>
