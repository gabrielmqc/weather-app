<template>
  <div v-if="weatherData.length" class="weather-chart">
    <h2>Gráfico de Temperaturas</h2>
    <canvas ref="chartCanvas"></canvas>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { Chart, registerables } from 'chart.js'
import type { WeatherRecord } from '@/@types/WeatherRecordData'

Chart.register(...registerables)

const { weatherData } = defineProps<{
  weatherData: WeatherRecord[]
}>()

const chartCanvas = ref<HTMLCanvasElement>()
let chart: Chart | null = null

function createChart() {
  if (!chartCanvas.value || !weatherData.length) return

  const ctx = chartCanvas.value.getContext('2d')
  if (!ctx) return

  if (chart) {
    chart.destroy()
  }

  const sortedData = [...weatherData].sort(
    (a, b) => new Date(a.recordedAt).getTime() - new Date(b.recordedAt).getTime(),
  )

  chart = new Chart(ctx, {
    type: 'line',
    data: {
      labels: sortedData.map((d) => new Date(d.recordedAt).toLocaleDateString('pt-BR')),
      datasets: [
        {
          label: 'Temperatura (°C)',
          data: sortedData.map((d) => d.temperature),
          borderColor: '#e74c3c',
          backgroundColor: 'rgba(231, 76, 60, 0.1)',
          tension: 0.4,
          fill: true,
        },
      ],
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          display: true,
          position: 'top',
        },
        title: {
          display: true,
          text: weatherData[0]?.city || 'Histórico de Temperaturas',
        },
      },
      scales: {
        y: {
          beginAtZero: false,
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

onMounted(() => {
  createChart()
})

watch(
  () => weatherData,
  () => {
    createChart()
  },
  { deep: true },
)
</script>

<style scoped>
.weather-chart {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-top: 2rem;
}

canvas {
  max-height: 400px;
}
</style>
