"use client"

import { useState, useEffect } from "react"
import type { Ticket } from "@/types"
import { ticketsApi } from "@/lib/api"

export function useTickets() {
  const [tickets, setTickets] = useState<Ticket[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetchTickets()
  }, [])

  const fetchTickets = async () => {
    try {
      setLoading(true)
      const response = await ticketsApi.getAll()
      setTickets(response.data)
      setError(null)
    } catch (err) {
      setError("Failed to fetch tickets")
      console.error("Error fetching tickets:", err)
    } finally {
      setLoading(false)
    }
  }

  const createTicket = async (ticket: Omit<Ticket, "id" | "createdAt" | "comments">) => {
    try {
      const response = await ticketsApi.create(ticket)
      setTickets((prev) => [...prev, response.data])
      return response.data
    } catch (err) {
      setError("Failed to create ticket")
      throw err
    }
  }

  const updateTicket = async (id: string, updates: Partial<Ticket>) => {
    try {
      const response = await ticketsApi.update(id, updates)
      setTickets((prev) => prev.map((t) => (t.id === id ? response.data : t)))
      return response.data
    } catch (err) {
      setError("Failed to update ticket")
      throw err
    }
  }

  const addComment = async (ticketId: string, comment: string) => {
    try {
      const response = await ticketsApi.addComment(ticketId, comment)
      setTickets((prev) => prev.map((t) => (t.id === ticketId ? response.data : t)))
      return response.data
    } catch (err) {
      setError("Failed to add comment")
      throw err
    }
  }

  return {
    tickets,
    loading,
    error,
    createTicket,
    updateTicket,
    addComment,
    refreshTickets: fetchTickets,
  }
}
