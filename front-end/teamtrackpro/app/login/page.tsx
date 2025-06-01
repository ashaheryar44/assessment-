"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { Container, Paper, TextField, Button, Typography, Box, Alert, Link, Divider, Chip } from "@mui/material"
import { useAuth } from "@/hooks/use-auth"

export default function LoginPage() {
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const router = useRouter()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError("")

    try {
      const user = await login(username, password)
      if (user) {
        // Redirect based on role
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
        }
      }
    } catch (err) {
      setError("Invalid username or password")
    } finally {
      setLoading(false)
    }
  }

  const demoCredentials = [
    { role: "Admin", username: "admin", password: "admin123" },
    { role: "Manager", username: "manager1", password: "manager123" },
    { role: "Developer", username: "dev1", password: "dev123" },
  ]

  return (
    <Container component="main" maxWidth="sm">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Paper elevation={3} sx={{ padding: 4, width: "100%" }}>
          <Typography component="h1" variant="h4" align="center" gutterBottom>
            TeamTrackPro
          </Typography>
          <Typography variant="h6" align="center" color="text.secondary" gutterBottom>
            Sign in to your account
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Box component="form" onSubmit={handleSubmit} sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="username"
              label="Username"
              name="username"
              autoComplete="username"
              autoFocus
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
            <TextField
              margin="normal"
              required
              fullWidth
              name="password"
              label="Password"
              type="password"
              id="password"
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
            <Button type="submit" fullWidth variant="contained" sx={{ mt: 3, mb: 2 }} disabled={loading}>
              {loading ? "Signing In..." : "Sign In"}
            </Button>

            <Box textAlign="center">
              <Link href="/reset-password" variant="body2">
                Forgot your password?
              </Link>
            </Box>
          </Box>

          <Divider sx={{ my: 3 }}>
            <Chip label="Demo Credentials" />
          </Divider>

          <Box sx={{ mt: 2 }}>
            <Typography variant="subtitle2" gutterBottom>
              Demo Login Credentials:
            </Typography>
            {demoCredentials.map((cred, index) => (
              <Box key={index} sx={{ mb: 1, p: 1, bgcolor: "grey.50", borderRadius: 1 }}>
                <Typography variant="body2">
                  <strong>{cred.role}:</strong> {cred.username} / {cred.password}
                </Typography>
              </Box>
            ))}
          </Box>
        </Paper>
      </Box>
    </Container>
  )
}
