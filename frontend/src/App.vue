<template>
  <div id="app">
    <Navbar />
    <router-view />
  </div>
</template>

<script setup lang="ts">
import Navbar from '@/components/AppNavbar.vue'
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: #f5f5f5;
  color: #333;
}

#app {
  min-height: 100vh;
}
</style>
