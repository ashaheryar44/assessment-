export interface User {
  id: string
  username: string
  email: string
  firstName: string
  lastName: string
  role: "admin" | "manager" | "developer"
  createdAt: string
}

export interface UserWithPassword extends User {
  password: string
}

export interface Project {
  id: string
  name: string
  description: string
  startDate: string
  endDate: string
  managerId: string
  status: "active" | "completed" | "on-hold"
  createdAt: string
}

export interface Ticket {
  id: string
  title: string
  description: string
  status: "todo" | "inprogress" | "qa" | "done"
  priority: "low" | "medium" | "high" | "critical"
  dueDate: string
  timeSpent: number
  projectId: string
  assignedTo: string
  type: "bug" | "feature" | "task" | "improvement"
  comments: Comment[]
  createdAt: string
}

export interface Comment {
  id: string
  text: string
  userId: string
  createdAt: string
}
