"use client"

import { useState, useEffect } from "react"
import type { Project } from "@/types"
import { mockProjects } from "@/data/mock-data"

export function useProjects() {
  const [projects, setProjects] = useState<Project[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setProjects(mockProjects)
      setLoading(false)
    }, 500)
  }, [])

  const createProject = (project: Omit<Project, "id" | "createdAt">) => {
    const newProject: Project = {
      ...project,
      id: Date.now().toString(),
      createdAt: new Date().toISOString(),
    }
    setProjects((prev) => [...prev, newProject])
    return newProject
  }

  const updateProject = (id: string, updates: Partial<Project>) => {
    setProjects((prev) => prev.map((p) => (p.id === id ? { ...p, ...updates } : p)))
  }

  return {
    projects,
    loading,
    createProject,
    updateProject,
  }
}
