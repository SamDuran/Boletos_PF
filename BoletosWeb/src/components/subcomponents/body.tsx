import CompraDialog from '../compraDialog';
import './body.style.css'
import { useState, useEffect } from 'react';
import { Eventos } from '../../../models/Eventos';
import { BoletosComprados } from '../../../models/Boletos';

function Body({ lista, OnBoletosAgregados }: { lista: Eventos[], OnBoletosAgregados: (boletos: BoletosComprados[]) => void }) {
    return (
        <>
            <div className="event-catalog">
                {lista.map((evento) => (
                    <EventoCard key={evento.eventoId} evento={evento} OnBoletosAgregados={OnBoletosAgregados} />
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
    const unsafeImg = window.URL.createObjectURL(new Blob([evento.foto]));

    return (
        <>
            <div className='card-bg' onClick={handleComponentClick} >
                
                {evento.foto != null && <img src={unsafeImg} alt='' className='event-image'></img>}

                {evento.foto == null && <img src='/src/components/resources/photo.png' className='event-image'></img>}

                <div className='divider' />
                <div className='headers-row' >
                    <span className='event-title'>{evento.nombreEvento}</span>
                    <span className='event-title'>Creado por: {evento.creador.userNombre}</span>
                </div>
                <span className='event-category'>({evento.categoriaEventos.categoria})</span>
            </div>
            {dialogVisible && <CompraDialog evento={evento} onClose={closeDialog} opened={dialogVisible} OnBoletosAgregados={OnBoletosAgregados} />}
        </>
    );
}