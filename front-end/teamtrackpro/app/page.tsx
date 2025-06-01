"use client"

import { useEffect } from "react"
import { useRouter } from "next/navigation"
import { useAuth } from "@/hooks/use-auth"
import { CircularProgress, Box } from "@mui/material"

export default function HomePage() {
  const { user, loading } = useAuth()
  const router = useRouter()

  useEffect(() => {
    if (!loading) {
      if (user) {
        // Redirect based on user role
        switch (user.role) {
          case "admin":
            router.push("/admin/dashboard")
            break
          case "manager":
            router.push("/manager/dashboard")
            break
          case "developer":
            router.push("/developer/dashboard")
            break
          default:
            router.push("/login")
        }
      } else {
        router.push("/login")
      }
    }
  }, [user, loading, router])

  return (
    <Box display="flex" justifyContent="center" alignItems="center" minHeight="100vh">
      <CircularProgress />
    </Box>
  )
}
