<template>
  <nav class="navbar">
    <div class="navbar__container">
      <router-link to="/" class="navbar__brand">
        <i class="pi pi-cloud"></i>
        Weather App
      </router-link>

      <div class="navbar__links">
        <router-link to="/" class="navbar__link" exact-active-class="active">
          <i class="pi pi-home"></i>
          Home
        </router-link>
        <router-link to="/weather" class="navbar__link" active-class="active">
          <i class="pi pi-search"></i>
          Consultar
        </router-link>
        <router-link to="/weather/history" class="navbar__link" active-class="active">
          <i class="pi pi-history"></i>
          Histórico
        </router-link>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import 'primeicons/primeicons.css'
</script>

<style scoped>
.navbar {
  background: white;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 1000;
}

.navbar__container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 64px;
}

.navbar__brand {
  font-size: 1.5rem;
  font-weight: bold;
  color: #2c3e50;
  text-decoration: none;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.navbar__brand i {
  color: #42b883;
  font-size: 1.5rem;
}

.navbar__links {
  display: flex;
  gap: 1.5rem;
}

.navbar__link {
  color: #666;
  text-decoration: none;
  padding: 0.5rem 0;
  border-bottom: 2px solid transparent;
  transition: all 0.3s;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.95rem;
}

.navbar__link i {
  font-size: 1rem;
  transition: color 0.3s;
}

.navbar__link:hover,
.navbar__link.active {
  color: #42b883;
  border-bottom-color: #42b883;
}

.navbar__link:hover i,
.navbar__link.active i {
  color: #42b883;
}

@media (max-width: 768px) {
  .navbar__container {
    flex-direction: column;
    height: auto;
    padding: 1rem;
    gap: 1rem;
  }

  .navbar__links {
    gap: 1rem;
    flex-wrap: wrap;
    justify-content: center;
  }

  .navbar__brand {
    font-size: 1.25rem;
  }
}
</style>
