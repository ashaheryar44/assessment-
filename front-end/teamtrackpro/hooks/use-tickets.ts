"use client"

import { useState, useEffect } from "react"
import type { Ticket } from "@/types"
import { mockTickets } from "@/data/mock-data"

export function useTickets() {
  const [tickets, setTickets] = useState<Ticket[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setTickets(mockTickets)
      setLoading(false)
    }, 500)
  }, [])

  const createTicket = (ticket: Omit<Ticket, "id" | "createdAt" | "comments">) => {
    const newTicket: Ticket = {
      ...ticket,
      id: Date.now().toString(),
      comments: [],
      createdAt: new Date().toISOString(),
    }
    setTickets((prev) => [...prev, newTicket])
    return newTicket
  }

  const updateTicket = (id: string, updates: Partial<Ticket>) => {
    setTickets((prev) => prev.map((t) => (t.id === id ? { ...t, ...updates } : t)))
  }

  const addComment = (ticketId: string, comment: string, userId: string) => {
    const newComment = {
      id: Date.now().toString(),
      text: comment,
      userId,
      createdAt: new Date().toISOString(),
    }

    setTickets((prev) => prev.map((t) => (t.id === ticketId ? { ...t, comments: [...t.comments, newComment] } : t)))
  }

  return {
    tickets,
    loading,
    createTicket,
    updateTicket,
    addComment,
  }
}
