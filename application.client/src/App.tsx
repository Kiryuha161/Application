import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Register from './components/Register'

function App() {
    const onSuccess = () => {

    }

    const onSwitchToLogin = () => {

    }

    return (
        <>
            <Register onSuccess={onSuccess} onSwitchToLogin={onSwitchToLogin} />
        </>
    )
}

export default App
