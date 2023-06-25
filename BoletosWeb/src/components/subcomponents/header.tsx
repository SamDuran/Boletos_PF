/* import React, { useState } from 'react'; */
import './header.css'
function Header() {

    /* const [state, setState] = useState("");

    const newState = () => {
        setState("a");
    }; */

    return (
        <>
            <div className="header">
                <div className="header-bg">
                    <img className="main-icon" src="/src/assets/BoletosPF_Icon.png" />
                    <div>
                        <img className="main-icon" src="/src/assets/BoletosPF_Icon.png" />
                        <div className="ticket-tracker">Ticket Tracker</div>
                        <div>¡Compra tus boletos sin límites, sin estrés!</div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Header