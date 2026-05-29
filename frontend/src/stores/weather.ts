import type { CoordinatesData, CoordinatesDataToSend } from '@/@types/CoordinatesData'
import type { WeatherHistoryQuery, WeatherRecord } from '@/@types/WeatherRecordData'
import { locationService } from '@/services/locationService'
import { weatherService } from '@/services/weatherService'
import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useWeatherStore = defineStore('weather', () => {
  const currentWeather = ref<WeatherRecord | null>(null)
  const weatherHistory = ref<WeatherRecord[]>([])
  const cityResults = ref<CoordinatesData[]>([])
  const registeredCities = ref<string[]>([])
  const selectedCity = ref<CoordinatesData | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  function setError(err: unknown) {
    if (err instanceof Error) {
      error.value = err.message
    } else if (typeof err === 'string') {
      error.value = err
    } else {
      error.value = 'Ocorreu um erro inesperado'
    }
  }

  async function searchCities(city: string) {
    loading.value = true
    error.value = null
    cityResults.value = []

    try {
      cityResults.value = await locationService.searchCities(city)

      if (cityResults.value.length === 0) {
        setError(`Nenhuma cidade encontrada para "${city}"`)
      }

      return cityResults.value
    } catch (err) {
      setError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  function selectCity(city: CoordinatesData) {
    selectedCity.value = city
    cityResults.value = []
  }

  async function searchByCity(city: string) {
    loading.value = true
    error.value = null

    try {
      currentWeather.value = await weatherService.getByCity(city)
      return currentWeather.value
    } catch (err) {
      setError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function searchByCoordinates(lat: number, lon: number, cityName?: string) {
    loading.value = true
    error.value = null

    try {
      currentWeather.value = await weatherService.getByCoordinates({
        lat,
        lon,
      } as CoordinatesDataToSend)

      if (cityName && currentWeather.value) {
        currentWeather.value = {
          ...currentWeather.value,
          city: cityName,
        }
      }

      return currentWeather.value
    } catch (err) {
      setError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchHistory(query: WeatherHistoryQuery) {
    loading.value = true
    error.value = null

    try {
      weatherHistory.value = await weatherService.getHistory(query)
      return weatherHistory.value
    } catch (err) {
      setError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchRegisteredCities() {
    loading.value = true
    error.value = null

    try {
      registeredCities.value = await weatherService.getAllCitiesRegistered()
      return registeredCities.value
    } catch (err) {
      setError(err)
      throw err
    } finally {
      loading.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  return {
    currentWeather,
    weatherHistory,
    cityResults,
    registeredCities,
    selectedCity,
    loading,
    error,
    searchCities,
    selectCity,
    searchByCity,
    searchByCoordinates,
    fetchHistory,
    fetchRegisteredCities,
    clearError,
  }
})
