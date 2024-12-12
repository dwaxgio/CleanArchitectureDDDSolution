<template>
  <div class="container">
    <input v-model="filter" placeholder="Filter by name" />
    <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Country</th>
          <th>City</th>
          <th>Email</th>
          <th>Picture</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in paginatedUsers" :key="user.email">
          <td>{{ user.name }}</td>
          <td>{{ user.country }}</td>
          <td>{{ user.city }}</td>
          <td>{{ user.email }}</td>
          <td><img :src="user.pictureUrl" alt="Picture" /></td>
        </tr>
      </tbody>
    </table>
    <div class="pagination">
      <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
      <span>Page {{ currentPage }} of {{ totalPages }}</span>
      <button @click="nextPage" :disabled="currentPage === totalPages">
        Next
      </button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import axios from "axios";

interface User {
  name: string;
  country: string;
  city: string;
  email: string;
  pictureUrl: string;
}

export default defineComponent({
  setup() {
    const users = ref<User[]>([]);
    const filter = ref("");
    const currentPage = ref(1);
    const itemsPerPage = 5;

    const fetchUsers = async () => {
      try {
        const response = await axios.get("https://localhost:7007/Users");
        users.value = response.data;
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };

    fetchUsers();

    const filteredUsers = computed(() =>
      users.value.filter((u) =>
        u.name.toLowerCase().includes(filter.value.toLowerCase())
      )
    );

    const paginatedUsers = computed(() => {
      const start = (currentPage.value - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      return filteredUsers.value.slice(start, end);
    });

    const totalPages = computed(() =>
      Math.ceil(filteredUsers.value.length / itemsPerPage)
    );

    const nextPage = () => {
      if (currentPage.value < totalPages.value) {
        currentPage.value++;
      }
    };

    const prevPage = () => {
      if (currentPage.value > 1) {
        currentPage.value--;
      }
    };

    return {
      filter,
      users,
      filteredUsers,
      paginatedUsers,
      currentPage,
      totalPages,
      nextPage,
      prevPage,
    };
  },
});
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  text-align: center;
}

input {
  margin-bottom: 20px;
  padding: 10px;
  width: 100%;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 16px;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin: 0 auto;
}

th,
td {
  padding: 10px;
  text-align: left;
  border: 1px solid #ddd;
}

th {
  background-color: #f4f4f4;
  font-weight: bold;
}

td img {
  width: 50px;
  height: 50px;
  border-radius: 50%;
}

.pagination {
  margin-top: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
}

button {
  padding: 10px 15px;
  border: none;
  border-radius: 5px;
  background-color: #007bff;
  color: white;
  cursor: pointer;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

button:hover:not(:disabled) {
  background-color: #0056b3;
}

@media (max-width: 600px) {
  table {
    font-size: 14px;
  }

  td img {
    width: 40px;
    height: 40px;
  }

  button {
    font-size: 14px;
    padding: 8px 10px;
  }
}
</style>
