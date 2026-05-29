import type { CoordinatesData } from '@/@types/CoordinatesData'
import { api } from '@/lib/api'

export const locationService = {
  async searchCities(city: string): Promise<CoordinatesData[]> {
    const response = await api.get<CoordinatesData[]>(
      `/api/location/city?city=${encodeURIComponent(city)}`,
    )
    return response.data
  },
}
