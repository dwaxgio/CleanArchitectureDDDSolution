<template>
  <div>
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
        <tr v-for="user in filteredUsers" :key="user.email">
          <td>{{ user.name }}</td>
          <td>{{ user.country }}</td>
          <td>{{ user.city }}</td>
          <td>{{ user.email }}</td>
          <td><img :src="user.pictureUrl" alt="Picture" /></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";

export default defineComponent({
  setup() {
    const users = ref([]);
    const filter = ref("");

    const fetchUsers = async () => {
      const response = await axios.get("http://localhost:5000/Users");
      users.value = response.data;
    };

    fetchUsers();

    const filteredUsers = computed(() =>
      users.value.filter((u) =>
        u.name.toLowerCase().includes(filter.value.toLowerCase())
      )
    );

    return { users, filter, filteredUsers };
  },
});
</script>
