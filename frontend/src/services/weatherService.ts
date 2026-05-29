import type { CoordinatesData, CoordinatesDataToSend } from '@/@types/CoordinatesData'
import type { WeatherHistoryQuery, WeatherRecord } from '@/@types/WeatherRecordData'
import { api } from '@/lib/api'

export const weatherService = {
  async getByCity(city: string): Promise<WeatherRecord> {
    const response = await api.post<WeatherRecord>(
      `/api/weather/city?city=${encodeURIComponent(city)}`,
    )
    return response.data
  },

  async getByCoordinates(coordinates: CoordinatesDataToSend): Promise<WeatherRecord> {
    const response = await api.post<WeatherRecord>(
      `/api/weather/coordinates?lat=${coordinates.lat}&lon=${coordinates.lon}`,
    )
    return response.data
  },

  async getHistory(query: WeatherHistoryQuery): Promise<WeatherRecord[]> {
    const params = new URLSearchParams()
    if (query.city) params.append('city', query.city)
    if (query.lat) params.append('lat', query.lat.toString())
    if (query.lon) params.append('lon', query.lon.toString())

    const response = await api.get<WeatherRecord[]>(`/api/weather/history?${params.toString()}`)
    return response.data
  },

  async getAllCitiesRegistered(): Promise<string[]> {
    const response = await api.get<string[]>(`/api/weather/cities`)
    return response.data
  },
}
