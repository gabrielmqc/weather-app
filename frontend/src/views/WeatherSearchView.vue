<template>
  <div class="weather-search">
    <h1>
      <i class="pi pi-search"></i>
      Consultar Clima
    </h1>

    <div class="search-methods">
      <div class="method-tabs">
        <button :class="{ active: searchMethod === 'city' }" @click="switchToCitySearch">
          <i class="pi pi-building"></i>
          Por Cidade
        </button>
        <button
          :class="{ active: searchMethod === 'coordinates' }"
          @click="searchMethod = 'coordinates'"
        >
          <i class="pi pi-map-marker"></i>
          Por Coordenadas
        </button>
      </div>

      <!-- Busca por Cidade -->
      <div v-if="searchMethod === 'city'" class="search-form">
        <!-- Passo 1: Buscar cidades -->
        <div v-if="!selectedCity && cityResults.length === 0" class="form-group">
          <label for="city">
            <i class="pi pi-building"></i>
            Nome da Cidade:
          </label>
          <div class="input-group">
            <span class="input-icon">
              <i class="pi pi-search"></i>
            </span>
            <input
              id="city"
              v-model="cityInput"
              type="text"
              placeholder="Ex: São Paulo, Cascavel"
              @keyup.enter="handleCitySearch"
            />
            <button @click="handleCitySearch" :disabled="loading" class="btn-primary">
              <i v-if="!loading" class="pi pi-search"></i>
              <i v-if="loading" class="pi pi-spin pi-spinner"></i>
              {{ loading ? 'Buscando...' : 'Buscar Cidades' }}
            </button>
          </div>
        </div>

        <!-- Passo 2: Selecionar cidade dos resultados -->
        <div v-if="cityResults.length > 0" class="city-results">
          <h3>
            <i class="pi pi-list"></i>
            Cidades Encontradas ({{ cityResults.length }})
          </h3>
          <p class="hint">Selecione a cidade desejada para consultar o clima:</p>

          <div class="city-list">
            <div
              v-for="city in cityResults"
              :key="`${city.lat}-${city.lon}`"
              class="city-item"
              @click="handleSelectCity(city)"
            >
              <div class="city-item__main">
                <div class="city-item__name">
                  <i class="pi pi-map-marker city-icon"></i>
                  {{ city.name }}
                </div>
                <div class="city-item__location">{{ city.state }}, {{ city.country }}</div>
              </div>
              <div class="city-item__coords">
                <span class="coord">
                  <i class="pi pi-arrow-right"></i>
                  {{ city.lat.toFixed(4) }}
                </span>
                <span class="coord">
                  <i class="pi pi-arrow-up"></i>
                  {{ city.lon.toFixed(4) }}
                </span>
              </div>
              <div class="city-item__arrow">
                <i class="pi pi-chevron-right"></i>
              </div>
            </div>
          </div>

          <button @click="clearCitySearch" class="btn-secondary">
            <i class="pi pi-arrow-left"></i>
            Buscar outra cidade
          </button>
        </div>

        <!-- Cidade selecionada (após escolher) -->
        <div v-if="selectedCity && !loading && !error" class="selected-city">
          <div class="selected-city__info">
            <i class="pi pi-check-circle selected-icon"></i>
            <div>
              <strong>{{ selectedCity.name }}</strong>
              <span class="selected-details">
                {{ selectedCity.state }}, {{ selectedCity.country }} - Lat:
                {{ selectedCity.lat.toFixed(4) }}, Lon: {{ selectedCity.lon.toFixed(4) }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- Busca por Coordenadas -->
      <div v-if="searchMethod === 'coordinates'" class="search-form">
        <div class="form-group">
          <div class="coordinates-grid">
            <div>
              <label for="lat">
                <i class="pi pi-arrow-right"></i>
                Latitude:
              </label>
              <input
                id="lat"
                v-model.number="latInput"
                type="number"
                step="any"
                placeholder="Ex: -23.5505"
              />
            </div>
            <div>
              <label for="lon">
                <i class="pi pi-arrow-up"></i>
                Longitude:
              </label>
              <input
                id="lon"
                v-model.number="lonInput"
                type="number"
                step="any"
                placeholder="Ex: -46.6333"
              />
            </div>
          </div>
          <button
            @click="handleCoordinatesSearch"
            :disabled="loading"
            class="btn-primary"
            style="margin-top: 1rem"
          >
            <i v-if="!loading" class="pi pi-search"></i>
            <i v-if="loading" class="pi pi-spin pi-spinner"></i>
            {{ loading ? 'Buscando...' : 'Buscar Clima' }}
          </button>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="loading">
      <div class="loading-icon">
        <i class="pi pi-spin pi-spinner" style="font-size: 3rem; color: #42b883"></i>
      </div>
      <p>{{ cityResults.length > 0 ? 'Consultando clima...' : 'Buscando cidades...' }}</p>
    </div>

    <!-- Erro -->
    <div v-if="error" class="error-message">
      <i class="pi pi-exclamation-triangle"></i>
      {{ error }}
    </div>

    <!-- Resultado do Clima -->
    <div v-if="currentWeather && !loading" class="result">
      <h2>
        <i class="pi pi-check-circle"></i>
        Resultado da Consulta
      </h2>
      <WeatherCard :weather="currentWeather" detailed />

      <div class="result-actions">
        <router-link :to="`/weather/${currentWeather.id}`" class="btn-secondary">
          <i class="pi pi-info-circle"></i>
          Ver Detalhes
        </router-link>
        <router-link to="/weather/history" class="btn-secondary">
          <i class="pi pi-list"></i>
          Ver Histórico
        </router-link>
        <button @click="newSearch" class="btn-secondary">
          <i class="pi pi-plus"></i>
          Nova Consulta
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useWeather } from '@/composables/useWeather'
import WeatherCard from '@/components/WeatherCard.vue'
import 'primeicons/primeicons.css'
import type { CoordinatesData } from '@/@types/CoordinatesData'

const searchMethod = ref<'city' | 'coordinates'>('city')
const {
  cityInput,
  latInput,
  lonInput,
  currentWeather,
  cityResults,
  selectedCity,
  loading,
  error,
  searchCities,
  selectCity,
  searchByCoordinates,
  clearSearch,
} = useWeather()

async function handleCitySearch() {
  await searchCities()
}

async function handleSelectCity(city: CoordinatesData) {
  await selectCity(city)
}

async function handleCoordinatesSearch() {
  await searchByCoordinates()
}

function clearCitySearch() {
  clearSearch()
}

function switchToCitySearch() {
  searchMethod.value = 'city'
  clearSearch()
}

function newSearch() {
  clearSearch()
}
</script>

<style scoped>
.weather-search {
  max-width: 800px;
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

.method-tabs {
  display: flex;
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.method-tabs button {
  padding: 0.5rem 1.5rem;
  border: 2px solid #42b883;
  background: transparent;
  color: #42b883;
  cursor: pointer;
  border-radius: 4px;
  font-size: 1rem;
  transition: all 0.3s;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.method-tabs button.active {
  background: #42b883;
  color: white;
}

.method-tabs button.active i {
  color: white;
}

.method-tabs button i {
  color: #42b883;
  transition: color 0.3s;
}

.search-form {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 1.5rem;
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
  gap: 0.5rem;
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
  flex: 1;
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

.coordinates-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.btn-primary {
  padding: 0.75rem 1.5rem;
  background: #42b883;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background 0.3s;
  white-space: nowrap;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-primary:hover:not(:disabled) {
  background: #38a16f;
}

.btn-primary:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 0.75rem 1.5rem;
  background: transparent;
  color: #42b883;
  border: 2px solid #42b883;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  text-decoration: none;
  transition: all 0.3s;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-secondary:hover {
  background: #42b883;
  color: white;
}

/* City Results */
.city-results {
  margin-top: 1.5rem;
}

.city-results h3 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.city-results h3 i {
  color: #42b883;
}

.hint {
  color: #666;
  margin-bottom: 1rem;
  font-size: 0.9rem;
}

.city-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.city-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
  background: white;
}

.city-item:hover {
  border-color: #42b883;
  background: #f0faf5;
  transform: translateX(5px);
}

.city-item__main {
  flex: 1;
}

.city-item__name {
  font-weight: 600;
  color: #2c3e50;
  font-size: 1.1rem;
  margin-bottom: 0.25rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.city-icon {
  color: #42b883;
  font-size: 1rem;
}

.city-item__location {
  color: #666;
  font-size: 0.9rem;
  padding-left: 1.5rem;
}

.city-item__coords {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  margin-right: 1rem;
}

.coord {
  font-family: monospace;
  font-size: 0.8rem;
  color: #888;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.coord i {
  font-size: 0.7rem;
  color: #42b883;
}

.city-item__arrow {
  color: #42b883;
  font-size: 1.2rem;
}

.selected-city {
  background: #f0faf5;
  border: 1px solid #42b883;
  border-radius: 8px;
  padding: 1rem;
  margin-top: 1rem;
}

.selected-city__info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.selected-icon {
  color: #42b883;
  font-size: 1.5rem;
}

.selected-details {
  display: block;
  color: #666;
  font-size: 0.9rem;
  margin-top: 0.25rem;
}

.loading {
  text-align: center;
  padding: 3rem;
}

.loading-icon {
  margin-bottom: 1rem;
}

.error-message {
  background: #fee;
  color: #c33;
  padding: 1rem;
  border-radius: 4px;
  margin-top: 1rem;
  border-left: 4px solid #c33;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.error-message i {
  color: #c33;
}

.result {
  margin-top: 2rem;
}

.result h2 {
  color: #2c3e50;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.result h2 i {
  color: #42b883;
}

.result-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1.5rem;
  flex-wrap: wrap;
}
</style>
