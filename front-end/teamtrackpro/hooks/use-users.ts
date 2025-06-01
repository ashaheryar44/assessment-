"use client"

import { useState, useEffect } from "react"
import type { User, UserWithPassword } from "@/types"
import { mockUsers } from "@/data/mock-data"

export function useUsers() {
  const [users, setUsers] = useState<User[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    // Simulate API call and remove passwords
    setTimeout(() => {
      const usersWithoutPasswords = mockUsers.map(({ password, ...user }) => user)
      setUsers(usersWithoutPasswords)
      setLoading(false)
    }, 500)
  }, [])

  const createUser = (user: Omit<UserWithPassword, "id" | "createdAt">) => {
    const newUser: User = {
      id: Date.now().toString(),
      username: user.username,
      email: user.email,
      firstName: user.firstName,
      lastName: user.lastName,
      role: user.role,
      createdAt: new Date().toISOString(),
    }
    setUsers((prev) => [...prev, newUser])
    return newUser
  }

  const updateUser = (id: string, updates: Partial<User>) => {
    setUsers((prev) => prev.map((u) => (u.id === id ? { ...u, ...updates } : u)))
  }

  return {
    users,
    loading,
    createUser,
    updateUser,
  }
}
