<template>
  <div class="no-results">
    <div class="no-results-icon">
      <i class="pi pi-search" style="font-size: 4rem; color: #ccc"></i>
    </div>
    <h3>Nenhum registro encontrado</h3>
    <p>Não há registros de temperatura para esta localidade nos últimos 30 dias.</p>
    <router-link to="/weather" class="btn-primary">
      <i class="pi pi-plus"></i>
      Fazer Nova Consulta
    </router-link>
  </div>
</template>

<style scoped>
.no-results {
  text-align: center;
  padding: 3rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.no-results-icon {
  margin-bottom: 1rem;
}

.no-results h3 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.no-results p {
  color: #666;
  margin-bottom: 1.5rem;
}

.btn-primary {
  padding: 0.75rem 1.5rem;
  background: #42b883;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  transition: background 0.3s;
}

.btn-primary:hover {
  background: #38a16f;
}
</style>
