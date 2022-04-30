import { useTypedSelector } from "../../hooks/useTypedSelector";
import UploadForm from '../UploadForm/UploadForm'
import TranslatedContent from '../TranslatedContent/TranslatedContent'

const Account = () => {
    const {translated} = useTypedSelector(state => state)

    return (
        <div>
            {translated?.texts && (translated?.texts?.length > 0)
            ? (
                <TranslatedContent translated={translated.texts} />
            ) : (
                <UploadForm />
            )}
        </div>
    );
  }
  
  export default Account;