export interface WeatherRecord {
  id: string
  city: string
  temperature: number
  latitude: number
  longitude: number
  recordedAt: string
}

export interface WeatherHistoryQuery {
  city?: string
  lat?: number
  lon?: number
}
