<template>
  <div class="chart-section">
    <canvas ref="chartCanvas"></canvas>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted, onUnmounted } from 'vue'
import { Chart, registerables } from 'chart.js'
import type { WeatherRecord } from '@/@types/WeatherRecordData'

Chart.register(...registerables)

const props = defineProps<{
  history: WeatherRecord[]
}>()

const chartCanvas = ref<HTMLCanvasElement>()
let chart: Chart | null = null

function createChart() {
  if (!chartCanvas.value || !props.history.length) return

  const ctx = chartCanvas.value.getContext('2d')
  if (!ctx) return

  if (chart) {
    chart.destroy()
  }

  const data = [...props.history].sort(
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
          text: `Histórico de Temperaturas - ${props.history[0]?.city || ''}`,
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

watch(
  () => props.history,
  () => {
    setTimeout(createChart, 100)
  },
  { deep: true },
)

onMounted(() => {
  setTimeout(createChart, 100)
})

onUnmounted(() => {
  if (chart) {
    chart.destroy()
  }
})
</script>

<style scoped>
.chart-section {
  padding: 1.5rem;
  height: 400px;
}
</style>
