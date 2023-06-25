import './body.style.css'

function Body({lista}:{lista:Eventos[]}) {
    return (
        <>
            <div className="event-catalog">
                {lista.map((evento) => (
                    <EventoCard key={evento.eventoId} evento={evento} />
                ))}
            </div>
        </>
    );
}

export default Body


function EventoCard({evento}:{evento:Eventos}){
    return (
        <>
            <div className='card-bg'>
                <img src='/src/components/resources/photo.png' className='event-image'></img>
                <div className='divider'/>
                <span className='event-title'>{evento.nombreEvento}</span>
            </div>
        </>
    );
}