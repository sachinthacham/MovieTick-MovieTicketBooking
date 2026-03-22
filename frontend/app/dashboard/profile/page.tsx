import { Button } from "@/components/ui/Button"
import { Input } from "@/components/ui/Input"

export default function ProfilePage() {
  return (
    <div className="space-y-8 w-full max-w-2xl">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Profile Settings</h1>
        <p className="text-(--muted-foreground) text-sm mt-1">Update your personal details and preferences.</p>
      </div>

      <div className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8 space-y-6">
        <div className="flex items-center space-x-6 pb-6 border-b border-(--border)">
          <div className="h-20 w-20 rounded-full bg-(--primary) text-(--primary-foreground) flex items-center justify-center font-bold text-3xl shrink-0">
            JS
          </div>
          <div>
            <Button variant="outline" size="sm" className="mb-2">Change Avatar</Button>
            <p className="text-xs text-(--muted-foreground)">JPG, GIF or PNG. 1MB max.</p>
          </div>
        </div>

        <div className="space-y-4 pt-2">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="text-sm font-medium">First Name</label>
              <Input defaultValue="John" className="mt-2" />
            </div>
            <div>
              <label className="text-sm font-medium">Last Name</label>
              <Input defaultValue="Smith" className="mt-2" />
            </div>
          </div>

          <div>
            <label className="text-sm font-medium">Email Address</label>
            <Input type="email" defaultValue="john@example.com" className="mt-2" />
          </div>

          <div>
            <label className="text-sm font-medium">Phone Number</label>
            <Input type="tel" defaultValue="+91 9876543210" className="mt-2" />
          </div>
        </div>

        <div className="pt-6 flex justify-end">
          <Button size="lg" className="px-8">Save Changes</Button>
        </div>
      </div>

      {/* Password Reset Section */}
      <div className="bg-(--background) border border-(--border) rounded-2xl p-6 md:p-8 space-y-6">
        <div>
          <h2 className="text-lg font-bold">Change Password</h2>
          <p className="text-(--muted-foreground) text-sm">Ensure your account is using a long, random password to stay secure.</p>
        </div>

        <div className="space-y-4">
          <div>
            <label className="text-sm font-medium">Current Password</label>
            <Input type="password" placeholder="••••••••" className="mt-2 max-w-sm" />
          </div>
          <div>
            <label className="text-sm font-medium">New Password</label>
            <Input type="password" placeholder="••••••••" className="mt-2 max-w-sm" />
          </div>
          <div>
            <label className="text-sm font-medium">Confirm New Password</label>
            <Input type="password" placeholder="••••••••" className="mt-2 max-w-sm" />
          </div>
        </div>

        <div className="pt-2">
          <Button variant="outline">Update Password</Button>
        </div>
      </div>
    </div>
  )
}
