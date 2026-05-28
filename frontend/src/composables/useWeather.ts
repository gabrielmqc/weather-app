import { ref, computed } from 'vue'
import { useWeatherStore } from '@/stores/weather'
import type { CoordinatesData } from '@/@types/CoordinatesData'

export function useWeather() {
  const store = useWeatherStore()
  const cityInput = ref('')
  const latInput = ref<number>(0)
  const lonInput = ref<number>(0)

  const currentWeather = computed(() => store.currentWeather)
  const weatherHistory = computed(() => store.weatherHistory)
  const cityResults = computed(() => store.cityResults)
  const selectedCity = computed(() => store.selectedCity)
  const loading = computed(() => store.loading)
  const error = computed(() => store.error)

  const sortedHistory = computed(() => {
    return [...weatherHistory.value].sort(
      (a, b) => new Date(b.recordedAt).getTime() - new Date(a.recordedAt).getTime(),
    )
  })

  async function searchCities() {
    if (!cityInput.value.trim()) {
      store.error = 'Por favor, insira o nome da cidade'
      return
    }

    await store.searchCities(cityInput.value.trim())
  }

  function selectCity(city: CoordinatesData) {
    store.selectCity(city)

    return store.searchByCoordinates(city.lat, city.lon)
  }

  async function searchByCoordinates() {
    if (!latInput.value || !lonInput.value) {
      store.error = 'Por favor, insira latitude e longitude'
      return
    }

    await store.searchByCoordinates(latInput.value, lonInput.value)
  }

  function clearSearch() {
    store.cityResults = []
    store.selectedCity = null
    store.error = null
    cityInput.value = ''
  }

  return {
    cityInput,
    latInput,
    lonInput,
    currentWeather,
    weatherHistory,
    cityResults,
    selectedCity,
    loading,
    error,
    sortedHistory,
    searchCities,
    selectCity,
    searchByCoordinates,
    fetchHistory: store.fetchHistory,
    clearSearch,
  }
}
