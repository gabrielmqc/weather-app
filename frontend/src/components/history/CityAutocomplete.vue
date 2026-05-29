<template>
  <div class="input-with-autocomplete">
    <div class="input-group">
      <span class="input-icon">
        <i class="pi pi-search"></i>
      </span>
      <input
        :value="modelValue"
        @input="handleInput"
        type="text"
        placeholder="Digite o nome da cidade"
        @keyup.enter="handleEnter"
        @focus="showSuggestions = true"
        @blur="hideSuggestionsDelayed"
      />
    </div>

    <ul v-if="showSuggestions && filteredSuggestions.length > 0" class="suggestions-list">
      <li
        v-for="city in filteredSuggestions"
        :key="city"
        @mousedown.prevent="selectSuggestion(city)"
        class="suggestion-item"
      >
        <i class="pi pi-map-marker"></i>
        {{ city }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useWeather } from '@/composables/useWeather'

const props = defineProps<{
  modelValue: string
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
  search: [city: string]
}>()

const { registeredCities, fetchRegisteredCities } = useWeather()
const showSuggestions = ref(false)

onMounted(() => {
  fetchRegisteredCities()
})

const filteredSuggestions = computed(() => {
  if (!props.modelValue.trim()) return registeredCities.value

  const searchTerm = props.modelValue.toLowerCase().trim()
  return registeredCities.value.filter((city) => city.toLowerCase().includes(searchTerm))
})

function handleInput(event: Event) {
  const value = (event.target as HTMLInputElement).value
  emit('update:modelValue', value)
  showSuggestions.value = true
}

function handleEnter() {
  showSuggestions.value = false
  emit('search', props.modelValue)
}

function selectSuggestion(city: string) {
  emit('update:modelValue', city)
  showSuggestions.value = false
  emit('search', city)
}

function hideSuggestionsDelayed() {
  setTimeout(() => {
    showSuggestions.value = false
  }, 200)
}
</script>

<style scoped>
.input-with-autocomplete {
  position: relative;
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

input {
  width: 100%;
  padding: 0.75rem 0.75rem 0.75rem 2.25rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

input:focus {
  outline: none;
  border-color: #42b883;
}

.suggestions-list {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #ddd;
  border-radius: 0 0 4px 4px;
  max-height: 200px;
  overflow-y: auto;
  z-index: 1000;
  list-style: none;
  padding: 0;
  margin: 0;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.suggestion-item {
  padding: 0.5rem 0.75rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background 0.2s;
}

.suggestion-item:hover {
  background: #f0f0f0;
}

.suggestion-item i {
  color: #42b883;
}
</style>
