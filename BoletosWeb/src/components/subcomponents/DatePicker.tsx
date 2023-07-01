import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import './searchbar.css'


export function MyDatePicker({ className, placeholder, selectedDate, setSelectedDate }: { className: string, placeholder: string, selectedDate: Date | null, setSelectedDate: ((fec: Date | null) => void) }) {

    const handleDateChange = (date: Date | null) => {
        if (date != null)
            setSelectedDate(new Date(date));
        else
            setSelectedDate(null)
    };

    return (
        <div>
            <DatePicker className={className} dateFormat={'dd/MMM/yyyy'} isClearable selected={selectedDate} placeholderText={placeholder} onChange={handleDateChange} />
        </div>
    );
}
