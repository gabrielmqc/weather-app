<template>
  <div class="registered-cities">
    <div class="header">
      <button @click="loadCities" :disabled="loading" class="btn-load-cities">
        <i class="pi pi-list"></i>
        {{ cities.length > 0 ? 'Atualizar Lista' : 'Carregar Cidades Registradas' }}
      </button>
    </div>

    <div v-if="cities.length > 0" class="chips-container">
      <span class="registered-label">
        <i class="pi pi-database"></i>
        Cidades disponíveis:
      </span>
      <div class="cities-chips">
        <span v-for="city in cities" :key="city" @click="selectCity(city)" class="city-chip">
          <i class="pi pi-building"></i>
          {{ city }}
        </span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useWeather } from '@/composables/useWeather'

const emit = defineEmits<{
  select: [city: string]
}>()

const { registeredCities: cities, loading, fetchRegisteredCities } = useWeather()

onMounted(() => {
  loadCities()
})

async function loadCities() {
  try {
    await fetchRegisteredCities()
  } catch (error) {
    console.error('Erro ao carregar cidades:', error)
  }
}

function selectCity(city: string) {
  emit('select', city)
}
</script>

<style scoped>
.registered-cities {
  margin: 1rem 0;
}

.header {
  margin-bottom: 0.5rem;
}

.btn-load-cities {
  width: 100%;
  padding: 0.5rem;
  background: #f0f0f0;
  border: 1px dashed #ccc;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  transition: background 0.3s;
  color: #333;
}

.btn-load-cities:hover:not(:disabled) {
  background: #e0e0e0;
}

.btn-load-cities:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.registered-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  color: #333;
  font-size: 0.9rem;
}

.registered-label i {
  color: #42b883;
}

.cities-chips {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.city-chip {
  padding: 0.25rem 0.75rem;
  background: #f8f9fa;
  border: 1px solid #ddd;
  border-radius: 20px;
  cursor: pointer;
  font-size: 0.85rem;
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
  transition: all 0.3s;
}

.city-chip:hover {
  background: #42b883;
  color: white;
  border-color: #42b883;
}

.city-chip i {
  font-size: 0.8rem;
}
</style>
