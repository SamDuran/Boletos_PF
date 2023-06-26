import CompraDialog from '../compraDialog';
import './body.style.css'
import { useState } from 'react';
import { BoletosComprados} from '../../../models/Boletos';

function Body({ lista, OnBoletosAgregados}: { lista: Eventos[], OnBoletosAgregados: (boletos: BoletosComprados[]) => void }) {
    return (
        <>
            <div className="event-catalog">
                {lista.map((evento) => (
                    <EventoCard key={evento.eventoId} evento={evento} OnBoletosAgregados={OnBoletosAgregados}/>
                ))}
            </div>
        </>
    );
}

export default Body


function EventoCard({ evento, OnBoletosAgregados }: { evento: Eventos, OnBoletosAgregados: (boletos: BoletosComprados[]) => void }) {

    const [dialogVisible, setDialogVisible] = useState(false);

    const handleComponentClick = () => {
        setDialogVisible(true);
    };

    const closeDialog = () => {
        setDialogVisible(false);
    };
    return (
        <>
            <div className='card-bg' onClick={handleComponentClick} >
                <img src='/src/components/resources/photo.png' className='event-image'></img>
                <div className='divider' />
                <span className='event-title'>{evento.nombreEvento}</span>
            </div>
            {dialogVisible && <CompraDialog evento={evento} onClose={closeDialog} opened={dialogVisible} OnBoletosAgregados={OnBoletosAgregados}/>}
        </>
    );
}