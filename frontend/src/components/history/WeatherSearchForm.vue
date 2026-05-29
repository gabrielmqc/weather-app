<template>
  <div class="search-section">
    <div class="search-form">
      <div class="form-group">
        <label>
          <i class="pi pi-building"></i>
          Buscar por Cidade:
        </label>
        <CityAutocomplete v-model="searchCity" @search="handleCitySearch" />
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

      <CityChips @select="handleCitySelect" />

      <button @click="handleSearch" :disabled="loading" class="btn-search">
        <i v-if="!loading" class="pi pi-search"></i>
        <i v-if="loading" class="pi pi-spin pi-spinner"></i>
        {{ loading ? 'Buscando...' : 'Buscar Histórico' }}
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import type { WeatherHistoryQuery } from '@/@types/WeatherRecordData'
import CityAutocomplete from './CityAutocomplete.vue'
import CityChips from './CityChips.vue'

const emit = defineEmits<{
  search: [query: WeatherHistoryQuery]
}>()

defineProps<{
  loading?: boolean
}>()

const searchCity = ref('')
const searchLat = ref(0)
const searchLon = ref(0)

function handleCitySearch(city: string) {
  searchCity.value = city
  handleSearch()
}

function handleCitySelect(city: string) {
  searchCity.value = city
  handleSearch()
}

function handleSearch() {
  const query: WeatherHistoryQuery = {}

  if (searchCity.value.trim()) {
    query.city = searchCity.value.trim()
  } else if (searchLat.value || searchLon.value) {
    query.lat = searchLat.value
    query.lon = searchLon.value
  } else {
    return
  }

  emit('search', query)
}
</script>

<style scoped>
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
</style>
