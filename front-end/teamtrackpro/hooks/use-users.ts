"use client"

import { useState, useEffect } from "react"
import type { User, UserWithPassword } from "@/types"
import { usersApi } from "@/lib/api"

export function useUsers() {
  const [users, setUsers] = useState<User[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetchUsers()
  }, [])

  const fetchUsers = async () => {
    try {
      setLoading(true)
      const response = await usersApi.getAll()
      setUsers(response.data)
      setError(null)
    } catch (err) {
      setError("Failed to fetch users")
      console.error("Error fetching users:", err)
    } finally {
      setLoading(false)
    }
  }

  const createUser = async (user: Omit<UserWithPassword, "id" | "createdAt">) => {
    try {
      const response = await usersApi.create(user)
      setUsers((prev) => [...prev, response.data])
      return response.data
    } catch (err) {
      setError("Failed to create user")
      throw err
    }
  }

  const updateUser = async (id: string, updates: Partial<User>) => {
    try {
      const response = await usersApi.update(id, updates)
      setUsers((prev) => prev.map((u) => (u.id === id ? response.data : u)))
      return response.data
    } catch (err) {
      setError("Failed to update user")
      throw err
    }
  }

  return {
    users,
    loading,
    error,
    createUser,
    updateUser,
    refreshUsers: fetchUsers,
  }
}
