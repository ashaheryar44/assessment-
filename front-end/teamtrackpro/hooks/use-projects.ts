"use client"

import { useState, useEffect } from "react"
import type { Project } from "@/types"
import { projectsApi } from "@/lib/api"

export function useProjects() {
  const [projects, setProjects] = useState<Project[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetchProjects()
  }, [])

  const fetchProjects = async () => {
    try {
      setLoading(true)
      const response = await projectsApi.getAll()
      setProjects(response.data)
      setError(null)
    } catch (err) {
      setError("Failed to fetch projects")
      console.error("Error fetching projects:", err)
    } finally {
      setLoading(false)
    }
  }

  const createProject = async (project: Omit<Project, "id" | "createdAt">) => {
    try {
      const response = await projectsApi.create(project)
      setProjects((prev) => [...prev, response.data])
      return response.data
    } catch (err) {
      setError("Failed to create project")
      throw err
    }
  }

  const updateProject = async (id: string, updates: Partial<Project>) => {
    try {
      const response = await projectsApi.update(id, updates)
      setProjects((prev) => prev.map((p) => (p.id === id ? response.data : p)))
      return response.data
    } catch (err) {
      setError("Failed to update project")
      throw err
    }
  }

  return {
    projects,
    loading,
    error,
    createProject,
    updateProject,
    refreshProjects: fetchProjects,
  }
}
